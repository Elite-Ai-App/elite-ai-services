using Supabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using EliteAI.Infrastructure.Data;
using EliteAI.Application.Interfaces;
using EliteAI.Infrastructure.Repositories;
using System.Reflection;
using Microsoft.OpenApi.Models;
using System.Text;
using EliteAI.Application.Services;
using EliteAI.API.Services;
using RabbitMQ.Client;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
DotEnv.Load();

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
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
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
                },
                Scheme = "Bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

// Add Supabase client
var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL") ??
    throw new InvalidOperationException("SUPABASE_URL is not configured. Please add it to your .env file.");
var supabaseAnonKey = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY") ??
    throw new InvalidOperationException("SUPABASE_ANON_KEY is not configured. Please add it to your .env file.");
var supabaseJwtSecret = Environment.GetEnvironmentVariable("SUPABASE_JWT_SECRET") ??
    throw new InvalidOperationException("SUPABASE_JWT_SECRET is not configured. Please add it to your .env file.");

builder.Services.AddScoped<Client>(_ => new Client(supabaseUrl, supabaseAnonKey));

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(supabaseJwtSecret)),
            ValidIssuer = $"{supabaseUrl}/auth/v1",
            ValidAudience = "authenticated",
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("JWT Error: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });

// Configure Entity Framework
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SSL Mode=Require;Trust Server Certificate=true";

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
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// RabbitMQ Configuration
var rabbitMqUrl = Environment.GetEnvironmentVariable("RABBITMQ_URL");
var rabbitMqPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");

builder.Services.AddSingleton<IConnection>(sp =>
{
    var factory = new ConnectionFactory
    {
        Uri = new Uri(rabbitMqUrl ?? throw new InvalidOperationException("RABBITMQ_URL is not configured")),
        Password = rabbitMqPassword ?? throw new InvalidOperationException("RABBITMQ_PASSWORD is not configured")
    };
    return factory.CreateConnectionAsync().Result;
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

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
