using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class WorkoutPlanRepository : Repository<WorkoutPlan>, IWorkoutPlanRepository
{
    public WorkoutPlanRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WorkoutPlan>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Include(p => p.Schedules)
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<WorkoutPlan> AddScheduleAsync(Guid id, DayOfWeek dayOfWeek, string workoutPlanName, DateTime date)
    {
        var plan = await _dbSet
            .Include(p => p.Schedules)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (plan == null)
            throw new KeyNotFoundException($"WorkoutPlan with ID {id} not found.");

        var schedule = new WorkoutPlanSchedule
        {
            WorkoutPlanId = id,
            DayOfWeek = dayOfWeek,
            WorkoutPlanName = workoutPlanName,
            Date = date
        };

        plan.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
        return plan;
    }

    public async Task<IEnumerable<WorkoutPlan>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(p => p.Schedules)
            .Where(p => p.UserId == userId &&
                       p.StartDate <= endDate &&
                       p.EndDate >= startDate)
            .ToListAsync();
    }
} 