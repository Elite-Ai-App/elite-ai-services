using System.Text;
using System.Text.Json;
using EliteAI.Application.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace EliteAI.API.Services;

public class RabbitMQMessagePublisher : EliteAI.Application.Interfaces.IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMQMessagePublisher> _logger;

    public RabbitMQMessagePublisher(IConnection connection, ILogger<RabbitMQMessagePublisher> logger)
    {
        _connection = connection;
        _channel = _connection.CreateChannelAsync().Result;
        _logger = logger;

        // Declare exchanges
        _channel.ExchangeDeclareAsync("workout_generation", ExchangeType.Topic, durable: true);
        _channel.ExchangeDeclareAsync("workout_generation_results", ExchangeType.Topic, durable: true);

        // Declare queues
        _channel.QueueDeclareAsync("workout_generation_requests", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclareAsync("workout_generation_complete", durable: true, exclusive: false, autoDelete: false);
        _channel.QueueDeclareAsync("workout_generation_failed", durable: true, exclusive: false, autoDelete: false);

        // Bind queues to exchanges
        _channel.QueueBindAsync("workout_generation_requests", "workout_generation", "request");
        _channel.QueueBindAsync("workout_generation_complete", "workout_generation_results", "complete");
        _channel.QueueBindAsync("workout_generation_failed", "workout_generation_results", "failed");
    }

    public Task PublishWorkoutGenerationRequest(string userId)
    {
        var message = new { UserId = userId };
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublishAsync(
            exchange: "workout_generation",
            routingKey: "request",
            body: body);

        _logger.LogInformation("Published workout generation request for user {UserId}", userId);
        return Task.CompletedTask;
    }

    public Task PublishWorkoutGenerationComplete(string userId, string workoutPlanId)
    {
        var message = new { UserId = userId, WorkoutPlanId = workoutPlanId };
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublishAsync(
            exchange: "workout_generation_results",
            routingKey: "complete",
            body: body);

        _logger.LogInformation("Published workout generation complete for user {UserId}, plan {WorkoutPlanId}", userId, workoutPlanId);
        return Task.CompletedTask;
    }

    public Task PublishWorkoutGenerationFailed(string userId, string errorMessage)
    {
        var message = new { UserId = userId, ErrorMessage = errorMessage };
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublishAsync(
            exchange: "workout_generation_results",
            routingKey: "failed",
            body: body);

        _logger.LogError("Published workout generation failed for user {UserId}: {ErrorMessage}", userId, errorMessage);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Dispose();
    }
}