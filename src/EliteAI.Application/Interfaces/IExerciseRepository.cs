using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Interfaces;

public interface IExerciseRepository : IRepository<Exercise>
{
    Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type);
} 