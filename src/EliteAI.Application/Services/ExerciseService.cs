using EliteAI.Application.Interfaces;
using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing exercises.
/// </summary>
public class ExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;

    /// <summary>
    /// Creates a new instance of ExerciseService.
    /// </summary>
    /// <param name="exerciseRepository">The repository for exercise operations.</param>
    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    /// <summary>
    /// Retrieves an exercise by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the exercise.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the exercise or null if not found.</returns>
    public async Task<Exercise?> GetExerciseByIdAsync(Guid id)
    {
        return await _exerciseRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves exercises by their type.
    /// </summary>
    /// <param name="type">The type of exercises to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of exercises.</returns>
    public async Task<IEnumerable<Exercise>> GetExercisesByTypeAsync(ExerciseType type)
    {
        return await _exerciseRepository.GetByTypeAsync(type);
    }
} 