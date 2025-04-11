using EliteAI.WorkoutGenerator.Consumers;
using EliteAI.WorkoutGenerator.Services;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using Sentry;

namespace EliteAI.WorkoutGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = "https://543a01428bb0981002c3870c4ce87a96@o4509048983584768.ingest.de.sentry.io/45091310543176484";
            options.Debug = true;
            options.TracesSampleRate = 1.0;
            options.MaxBreadcrumbs = 200;
            options.AttachStacktrace = true;
            options.ShutdownTimeout = TimeSpan.FromSeconds(5);
            options.Environment = builder.Environment.EnvironmentName;

        });


        // Load environment variables from .env file
        var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        Console.WriteLine($"Looking for .env file at: {envPath}");
        if (File.Exists(envPath))
        {
            Console.WriteLine(".env file found");
            DotEnv.Load(new DotEnvOptions(
                envFilePaths: new[] { envPath },
                ignoreExceptions: false
            ));
        }
        else
        {
            Console.WriteLine(".env file not found!");
        }

        // Add services to the container.
        builder.Services.AddControllers();

        // Configuration
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        // RabbitMQ Configuration
        var rabbitMqUrl = Environment.GetEnvironmentVariable("RABBITMQ_URL");
        Console.WriteLine($"Initial RabbitMQ URL: {rabbitMqUrl}");

        if (string.IsNullOrEmpty(rabbitMqUrl))
        {
            var username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "elite_ai";
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "elite_ai_password";
            var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
            var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
            rabbitMqUrl = $"amqp://{username}:{password}@{host}:{port}";
            Console.WriteLine($"Constructed RabbitMQ URL: {rabbitMqUrl}");
        }

        Uri uri;
        try
        {
            uri = new Uri(rabbitMqUrl);
            Console.WriteLine($"Successfully parsed URI - Host: {uri.Host}, Port: {uri.Port}, UserInfo: {uri.UserInfo}");
        }
        catch (UriFormatException ex)
        {
            Console.WriteLine($"Failed to parse RabbitMQ URL: {ex.Message}");
            Console.WriteLine($"Attempted URL format: {rabbitMqUrl}");
            throw;
        }

        // RabbitMQ Connection
        builder.Services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {

                Uri = uri,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            var maxRetries = 5;
            var retryCount = 0;
            var delay = TimeSpan.FromSeconds(5);

            while (retryCount < maxRetries)
            {
                try
                {
                    Console.WriteLine($"Attempting to connect to RabbitMQ at {uri.Host}:{uri.Port} (Attempt {retryCount + 1}/{maxRetries})");
                    var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                    Console.WriteLine("Successfully connected to RabbitMQ");
                    return connection;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    if (retryCount == maxRetries)
                    {
                        Console.WriteLine($"Failed to connect to RabbitMQ after {maxRetries} attempts: {ex.Message}");
                        throw;
                    }
                    Console.WriteLine($"Connection attempt {retryCount} failed: {ex.Message}. Retrying in {delay.TotalSeconds} seconds...");
                    Thread.Sleep(delay);
                }
            }

            throw new Exception("Failed to establish RabbitMQ connection after all retry attempts");
        });

        // Health Checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>("database")
            .AddRabbitMQ(
                sp => sp.GetRequiredService<IConnection>(),
                name: "rabbitmq",
                tags: new[] { "messagebroker" });

        // Services
        builder.Services.AddScoped<IWorkoutGenerationService, WorkoutGenerationService>();
        builder.Services.AddScoped<IMessagePublisher, RabbitMQMessagePublisher>();

        // Consumers
        builder.Services.AddHostedService<WorkoutGenerationConsumer>();

        var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
        var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
        var dbName = Environment.GetEnvironmentVariable("DB_NAME");
        var dbUser = Environment.GetEnvironmentVariable("DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

        var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode=Require;Trust Server Certificate=true";

        // Database Context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == "development")
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        // Replace UseEndpoints with top-level route registrations
        app.MapControllers();

        app.MapHealthChecks("/health");

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("messagebroker") || check.Tags.Contains("database")
        });

        app.Run();
    }
}