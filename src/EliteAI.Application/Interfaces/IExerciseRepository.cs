using EliteAI.Domain.Entities;

namespace EliteAI.Application.Interfaces;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type);
} 