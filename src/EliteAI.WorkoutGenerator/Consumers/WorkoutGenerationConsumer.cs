using System.Text;
using System.Text.Json;
using EliteAI.WorkoutGenerator.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EliteAI.WorkoutGenerator.Consumers;

public class WorkoutGenerationConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WorkoutGenerationConsumer> _logger;

    public WorkoutGenerationConsumer(
        IConnection connection,
        IServiceProvider serviceProvider,
        ILogger<WorkoutGenerationConsumer> logger)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
        _logger = logger;
        _channel = _connection.CreateModel();

        // Ensure the queue exists
        _channel.QueueDeclare("workout_generation_requests", durable: true, exclusive: false, autoDelete: false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var request = JsonSerializer.Deserialize<WorkoutGenerationRequest>(message);

                if (request == null)
                {
                    _logger.LogError("Failed to deserialize workout generation request");
                    return;
                }

                _logger.LogInformation("Received workout generation request for user {UserId}", request.UserId);

                using var scope = _serviceProvider.CreateScope();
                var workoutService = scope.ServiceProvider.GetRequiredService<IWorkoutGenerationService>();
                var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();

                try
                {
                    await workoutService.GenerateWorkoutPlan(request.UserId);
                    await messagePublisher.PublishWorkoutGenerationComplete(request.UserId, "PLAN_ID"); // TODO: Get actual plan ID
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating workout plan for user {UserId}", request.UserId);
                    await messagePublisher.PublishWorkoutGenerationFailed(request.UserId, ex.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing workout generation request");
            }
            finally
            {
                _channel.BasicAck(ea.DeliveryTag, false);
            }
        };

        _channel.BasicConsume(
            queue: "workout_generation_requests",
            autoAck: false,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        base.Dispose();
    }
}

public class WorkoutGenerationRequest
{
    public string UserId { get; set; } = null!;
} 