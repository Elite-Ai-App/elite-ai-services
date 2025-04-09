using EliteAI.Domain.Entities;

namespace EliteAI.Application.Interfaces;

public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
{
    Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(Guid userId);
    Task<WorkoutPlan> AddScheduleAsync(Guid id, DayOfWeek dayOfWeek, string workoutPlanName, DateTime date);
    Task<IEnumerable<WorkoutPlan>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
} 