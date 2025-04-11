using Supabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using EliteAI.Infrastructure.Data;
using EliteAI.Application.Interfaces;
using EliteAI.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System.Text;
using EliteAI.Application.Services;
using EliteAI.API.Services;
using RabbitMQ.Client;
using dotenv.net;


var builder = WebApplication.CreateBuilder(args);

// Configure URLs
builder.WebHost.UseUrls("http://*:80");

// Configure Sentry first
builder.WebHost.UseSentry(options =>
{
    options.Dsn = "https://f2b9d60acaf1df75f83213dd061c3b4f@o4509048983584768.ingest.de.sentry.io/4509128272379984";
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


// Log startup
SentrySdk.CaptureMessage("Application starting up", SentryLevel.Info);

try
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddHttpClient();

    // Configure CORS for Swagger UI
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    SentrySdk.CaptureMessage("Configuring Swagger", SentryLevel.Info);
    builder.Services.AddSwaggerGen();

    // Configure Swagger/OpenAPI
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "EliteAI API",
            Version = "v1",
            Description = "API for EliteAI basketball training application"
        });

        // Add JWT Authentication
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });




    // Configure JWT Authentication
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("SUPABASE_JWT_ISSUER"),
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SUPABASE_JWT_SECRET")))
            };
        });

    // Configure Entity Framework
    SentrySdk.CaptureMessage("Configuring Database", SentryLevel.Info);
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbUser = Environment.GetEnvironmentVariable("DB_USER");
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

    var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode=Require;Trust Server Certificate=true";
    SentrySdk.CaptureMessage($"Database Connection String: {connectionString}", SentryLevel.Info);

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString, options =>
        {
            options.EnableRetryOnFailure(
                maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
            options.CommandTimeout(60);
            options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    });

    // Register AutoMapper
    builder.Services.AddAutoMapper(typeof(EliteAI.Application.Mapping.UserMappingProfile).Assembly);

    // RabbitMQ Configuration
    var rabbitMqUrl = Environment.GetEnvironmentVariable("RABBITMQ_URL");


    builder.Services.AddSingleton<IConnection>(sp =>
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(rabbitMqUrl ?? throw new InvalidOperationException("RABBITMQ_URL is not configured")),
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
        };
        return factory.CreateConnectionAsync().GetAwaiter().GetResult();
    });

    // Register services
    builder.Services.AddScoped<EliteAI.Application.Interfaces.IMessagePublisher, RabbitMQMessagePublisher>();

    //Repositories
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
    builder.Services.AddScoped<ISportsRepository, SportsRepository>();

    //Services
    builder.Services.AddScoped<OnboardingService>();
    builder.Services.AddScoped<UserService>();
    builder.Services.AddScoped<ProfileService>();
    builder.Services.AddScoped<SportsService>();

    SentrySdk.CaptureMessage("Seeting up Supabase", SentryLevel.Info);
    var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
    var supabaseAnonKey = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
    var supabaseAdminKey = Environment.GetEnvironmentVariable("SUPABASE_SERVICE_ROLE_KEY");



    var options = new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true,

        // SessionHandler = new SupabaseSessionHandler() <-- This must be implemented by the developer
    };

    // Note the creation as a singleton.
    builder.Services.AddSingleton(provider => new Supabase.Client(supabaseUrl, supabaseAdminKey));


    // Add logging
    builder.Services.AddLogging();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    // Use Sentry middleware
    app.UseSentryTracing();

    app.MapControllers();

    // Ensure database is created and migrations are applied
    SentrySdk.CaptureMessage("Applying Database Migrations", SentryLevel.Info);
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
            SentrySdk.CaptureMessage("Database Migrations Applied Successfully", SentryLevel.Info);
        }
        catch (Exception ex)
        {
            SentrySdk.CaptureException(ex);
            throw;
        }
    }

    SentrySdk.CaptureMessage("Application Started Successfully", SentryLevel.Info);
    app.Run();
}
catch (Exception ex)
{
    SentrySdk.CaptureException(ex);
    throw;
}
