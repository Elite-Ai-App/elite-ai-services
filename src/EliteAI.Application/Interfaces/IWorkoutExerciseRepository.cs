using EliteAI.Domain.Entities;

namespace EliteAI.Application.Interfaces;

public interface IWorkoutExerciseRepository : IRepository<WorkoutExercise>
{
    Task<IEnumerable<WorkoutExercise>> GetByWorkoutIdAsync(Guid workoutId);
    Task<WorkoutExercise> UpdateOrderAsync(Guid id, int order);
    Task<WorkoutExercise> UpdateSetsAndRepsAsync(Guid id, int sets, int reps);
} 