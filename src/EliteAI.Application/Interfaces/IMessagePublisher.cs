namespace EliteAI.Application.Interfaces;

/// <summary>
/// Interface for publishing messages to message queues
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a workout generation request for a specific user
    /// </summary>
    /// <param name="userId">The ID of the user to generate a workout for</param>
    Task PublishWorkoutGenerationRequest(string userId);

    /// <summary>
    /// Publishes a workout generation completion message
    /// </summary>
    /// <param name="userId">The ID of the user the workout was generated for</param>
    /// <param name="workoutPlanId">The ID of the generated workout plan</param>
    Task PublishWorkoutGenerationComplete(string userId, string workoutPlanId);

    /// <summary>
    /// Publishes a workout generation failure message
    /// </summary>
    /// <param name="userId">The ID of the user the workout generation failed for</param>
    /// <param name="errorMessage">The error message describing why the generation failed</param>
    Task PublishWorkoutGenerationFailed(string userId, string errorMessage);
} 