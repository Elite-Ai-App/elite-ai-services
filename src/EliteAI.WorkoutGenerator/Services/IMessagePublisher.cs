namespace EliteAI.WorkoutGenerator.Services;

public interface IMessagePublisher
{
    Task PublishWorkoutGenerationRequest(string userId);
    Task PublishWorkoutGenerationComplete(string userId, string workoutPlanId);
    Task PublishWorkoutGenerationFailed(string userId, string errorMessage);
} 