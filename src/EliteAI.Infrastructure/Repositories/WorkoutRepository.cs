using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class WorkoutRepository : Repository<Workout>, IWorkoutRepository
{
    public WorkoutRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Workout?> GetByScheduleIdAsync(Guid scheduleId)
    {
        return await _dbSet
            .Include(w => w.Exercises)
                .ThenInclude(e => e.Exercise)
            .Include(w => w.WorkoutLog)
            .FirstOrDefaultAsync(w => w.WorkoutPlanScheduleId == scheduleId);
    }

    public async Task<Workout> AssignToScheduleAsync(Guid id, Guid scheduleId)
    {
        var workout = await GetByIdAsync(id);
        if (workout == null)
            throw new KeyNotFoundException($"Workout with ID {id} not found.");

        workout.WorkoutPlanScheduleId = scheduleId;
        return await UpdateAsync(workout);
    }

    public async Task<Workout> RemoveFromScheduleAsync(Guid id)
    {
        var workout = await GetByIdAsync(id);
        if (workout == null)
            throw new KeyNotFoundException($"Workout with ID {id} not found.");

        workout.WorkoutPlanScheduleId = Guid.Empty;
        return await UpdateAsync(workout);
    }
} 