using EliteAI.WorkoutGenerator.Consumers;
using EliteAI.WorkoutGenerator.Services;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.WorkoutGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();

        // Configuration
        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

        // RabbitMQ Connection
        builder.Services.AddSingleton<IConnection>(sp =>
        {
            var factory = new ConnectionFactory
            {
                HostName = builder.Configuration["RabbitMQ:HostName"],
                UserName = builder.Configuration["RabbitMQ:UserName"],
                Password = builder.Configuration["RabbitMQ:Password"],
                Port = int.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672"),
                VirtualHost = builder.Configuration["RabbitMQ:VirtualHost"] ?? "/"
            };
            return await factory.CreateConnectionAsync();
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

        // Database Context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
            endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
            {
                Predicate = _ => false
            });
            endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains("messagebroker") || check.Tags.Contains("database")
            });
        });

        app.Run();
    }
} 