using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing player workouts.
/// </summary>
public class WorkoutService
{
    private readonly IWorkoutRepository _workoutRepository;

    /// <summary>
    /// Creates a new instance of WorkoutService.
    /// </summary>
    /// <param name="workoutRepository">The repository for player workout operations.</param>
    public WorkoutService(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    /// <summary>
    /// Retrieves a workout by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the workout.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the workout or null if not found.</returns>
    public async Task<Workout?> GetWorkoutByIdAsync(Guid id)
    {
        return await _workoutRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves a workout by its schedule ID.
    /// </summary>
    /// <param name="scheduleId">The unique identifier of the schedule.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the workout or null if not found.</returns>
    public async Task<Workout?> GetWorkoutByScheduleIdAsync(Guid scheduleId)
    {
        return await _workoutRepository.GetByScheduleIdAsync(scheduleId);
    }

    /// <summary>
    /// Creates a new workout.
    /// </summary>
    /// <param name="data">The workout data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created workout.</returns>
    public async Task<Workout> CreateWorkoutAsync(Workout data)
    {
        return await _workoutRepository.CreateAsync(data);
    }

    /// <summary>
    /// Updates an existing workout.
    /// </summary>
    /// <param name="id">The unique identifier of the workout to update.</param>
    /// <param name="data">The workout data to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated workout.</returns>
    public async Task<Workout> UpdateWorkoutAsync(Guid id, Workout data)
    {
        return await _workoutRepository.UpdateAsync(data);
    }

    /// <summary>
    /// Deletes a workout.
    /// </summary>
    /// <param name="id">The unique identifier of the workout to delete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deleted workout.</returns>
    public async Task DeleteWorkoutAsync(Guid id)
    {
        var workout = await  _workoutRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Workout with ID {id} not found.");

         await _workoutRepository.DeleteAsync(workout);
    }

    /// <summary>
    /// Assigns a workout to a schedule.
    /// </summary>
    /// <param name="id">The unique identifier of the workout.</param>
    /// <param name="scheduleId">The unique identifier of the schedule.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated workout.</returns>
    public async Task<Workout> AssignToScheduleAsync(Guid id, Guid scheduleId)
    {
        var workout = await _workoutRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Workout with ID {id} not found.");
        workout.WorkoutPlanScheduleId = scheduleId;
        return await _workoutRepository.UpdateAsync(workout);
    }       
} 