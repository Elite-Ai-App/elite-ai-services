using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class WorkoutExerciseRepository : Repository<WorkoutExercise>, IWorkoutExerciseRepository
{
    public WorkoutExerciseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WorkoutExercise>> GetByWorkoutIdAsync(Guid workoutId)
    {
        return await _dbSet
            .Include(e => e.Exercise)
            .Where(e => e.WorkoutId == workoutId)
            .OrderBy(e => e.Order)
            .ToListAsync();
    }

    public async Task<WorkoutExercise> UpdateOrderAsync(Guid id, int order)
    {
        var exercise = await GetByIdAsync(id);
        if (exercise == null)
            throw new KeyNotFoundException($"WorkoutExercise with ID {id} not found.");

        exercise.Order = order;
        return await UpdateAsync(exercise);
    }

    public async Task<WorkoutExercise> UpdateSetsAndRepsAsync(Guid id, int sets, int reps)
    {
        var exercise = await GetByIdAsync(id);
        if (exercise == null)
            throw new KeyNotFoundException($"WorkoutExercise with ID {id} not found.");

        exercise.Sets = sets;
        exercise.Reps = reps;
        return await UpdateAsync(exercise);
    }
} 