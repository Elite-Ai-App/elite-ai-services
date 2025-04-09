using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Interfaces;

public interface IWorkoutPlanRepository : IRepository<WorkoutPlan>
{
    Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(Guid userId);
    Task<WorkoutPlan> AddScheduleAsync(Guid id, TrainingDayOfWeek dayOfWeek, string workoutPlanName, DateTime date);
    Task<IEnumerable<WorkoutPlan>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
} 