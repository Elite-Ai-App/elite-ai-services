using EliteAI.Domain.Entities;

namespace EliteAI.Application.Interfaces;

public interface IWorkoutRepository : IRepository<Workout>
{
    Task<Workout?> GetByScheduleIdAsync(Guid scheduleId);
    Task<Workout> AssignToScheduleAsync(Guid id, Guid scheduleId);
    Task<Workout> RemoveFromScheduleAsync(Guid id);
} 