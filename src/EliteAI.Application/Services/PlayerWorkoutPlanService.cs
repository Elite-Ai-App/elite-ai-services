using EliteAI.Application.Interfaces;
using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing player workout plans.
/// </summary>
public class WorkoutPlanService
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;

    /// <summary>
    /// Creates a new instance of WorkoutPlanService.
    /// </summary>
    /// <param name="workoutPlanRepository">The repository for workout plan operations.</param>
    public WorkoutPlanService(IWorkoutPlanRepository workoutPlanRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
    }

    /// <summary>
    /// Retrieves a workout plan by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the workout plan.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the workout plan or null if not found.</returns>
    public async Task<WorkoutPlan?> GetWorkoutPlanByIdAsync(Guid id)
    {
        return await _workoutPlanRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves workout plans by user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of workout plans.</returns>
    public async Task<IEnumerable<WorkoutPlan>> GetWorkoutPlansByUserIdAsync(Guid userId)
    {
        return await _workoutPlanRepository.GetByUserIdAsync(userId);
    }

    /// <summary>
    /// Gets the current active workout plan for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the active workout plan or null if none exists.</returns>
    public async Task<WorkoutPlan?> GetCurrentActivePlanAsync(Guid userId)
    {
        var plans = await _workoutPlanRepository.GetByUserIdAsync(userId);
        var now = DateTime.UtcNow;

        return plans.FirstOrDefault(plan =>
            plan.StartDate <= now && plan.EndDate >= now);
    }

    /// <summary>
    /// Gets workouts within a specific date range.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of workout plans within the date range.</returns>
    public async Task<IEnumerable<WorkoutPlan>> GetWorkoutsByDateRangeAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate)
    {
        return await _workoutPlanRepository.GetByDateRangeAsync(userId, startDate, endDate);
    }

    /// <summary>
    /// Creates a new workout plan.
    /// </summary>
    /// <param name="data">The workout plan data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created workout plan.</returns>
    public async Task<WorkoutPlan> CreatePlanAsync(WorkoutPlan data)
    {
        return await _workoutPlanRepository.CreateAsync(data);
    }

    /// <summary>
    /// Adds a schedule to a workout plan.
    /// </summary>
    /// <param name="workoutPlanId">The ID of the workout plan.</param>
    /// <param name="TrainingDayOfWeek">The day of the week for the schedule.</param>
    /// <param name="workoutPlanName">The name of the workout for this schedule.</param>
    /// <param name="date">The date for this schedule.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created schedule.</returns>
    public async Task<WorkoutPlan> AddScheduleAsync(
        Guid workoutPlanId,
        TrainingDayOfWeek dayOfWeek,
        string workoutPlanName,
        DateTime date)
    {
        return await _workoutPlanRepository.AddScheduleAsync(
            workoutPlanId,
            dayOfWeek,
            workoutPlanName,
            date);
    }
} 