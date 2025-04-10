using EliteAI.WorkoutGenerator.Consumers;
using EliteAI.WorkoutGenerator.Services;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using dotenv.net;
using System;

namespace EliteAI.WorkoutGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load .env file
        DotEnv.Load(options: new DotEnvOptions(
            envFilePaths: new[] { ".env" },
            ignoreExceptions: false
        ));

        // Add services to the container.
        builder.Services.AddControllers();

        // Configuration
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        // Parse RabbitMQ URL
        var rabbitMqUrl = Environment.GetEnvironmentVariable("RABBITMQ_URL");
        if (string.IsNullOrEmpty(rabbitMqUrl))
            throw new Exception("RABBITMQ_URL is not configured");

        var uri = new Uri(rabbitMqUrl);
        var queueHostname = uri.Host;
        var queuePort = uri.Port;

        var queueUsername = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")
            ?? throw new Exception("No username provided");

        var queuePassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")
            ?? throw new Exception("No password provided");

        // RabbitMQ Connection
        builder.Services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = queueHostname,
                Port = queuePort,
                UserName = queueUsername,
                Password = queuePassword,
                VirtualHost = Environment.GetEnvironmentVariable("RABBITMQ_VHOST") ?? "/",
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            try
            {
                Console.WriteLine($"Connecting to RabbitMQ at {queueHostname}:{queuePort}");
                var connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                Console.WriteLine("Successfully connected to RabbitMQ");
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
                throw;
            }
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