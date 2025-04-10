using System.Text.Json;
using Anthropic;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;
using EliteAI.Domain.Helpers;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace EliteAI.WorkoutGenerator.Services;

public interface IWorkoutGenerationService
{
    Task GenerateWorkoutPlan(string userId);
}

public class WorkoutGenerationService : IWorkoutGenerationService
{
    private readonly ApplicationDbContext _context;
    private readonly AnthropicClient _anthropic;
    private readonly ILogger<WorkoutGenerationService> _logger;

    public WorkoutGenerationService(
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<WorkoutGenerationService> logger)
    {
        _context = context;
        _logger = logger;
        _anthropic = new AnthropicClient(configuration["Anthropic:ApiKey"]!);
    }

    public async Task GenerateWorkoutPlan(string userId)
    {

        if (!Guid.TryParse(userId, out var id)) throw new Exception("User Id Cannot be parsed");



        var user = await _context.Users
            .Include(u => u.Profile)
            .ThenInclude(p => p.Sports)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user?.Profile == null)
        {
            throw new InvalidOperationException("User or player profile not found");
        }

        var playerProfile = user.Profile;
        var playerSport = playerProfile.Sports.FirstOrDefault();

        if (playerSport == null)
        {
            throw new InvalidOperationException("Player sport not found");
        }


        //TODO REMAP THIS      

        var strengthExercises = await _context.Exercises
            .Where(e => e.Type == ExerciseType.STRENGTH &&
                (playerProfile.GymExperience == GymExperience.BEGINNER && e.Level == ExerciseLevel.BEGINNER) ||
                (playerProfile.GymExperience == GymExperience.INTERMEDIATE && (e.Level == ExerciseLevel.BEGINNER || e.Level == ExerciseLevel.INTERMEDIATE)) ||
                (playerProfile.GymExperience == GymExperience.ADVANCED))
            .ToListAsync();

        var mobilityExercises = await _context.Exercises
            .Where(e => e.Type == ExerciseType.MOBILITY &&
                (playerProfile.GymExperience == GymExperience.BEGINNER && e.Level == ExerciseLevel.BEGINNER) ||
                (playerProfile.GymExperience == GymExperience.INTERMEDIATE && (e.Level == ExerciseLevel.BEGINNER || e.Level == ExerciseLevel.INTERMEDIATE)) ||
                (playerProfile.GymExperience == GymExperience.ADVANCED))
            .ToListAsync();

        var exerciseDatabasePrompt = $@"
-Use only exercises from this database:
{string.Join("\n", strengthExercises.Select(ex => $"- [{ex.Id}] | {ex.Name} ({string.Join(", ", ex.MuscleGroup)}, Level: {ex.Level})"))}

-For mobility days, use these exercises:
{string.Join("\n", mobilityExercises.Select(ex => $"- [{ex.Id}] | {ex.Name} ({string.Join(", ", ex.MuscleGroup)})"))}";

        var schemaPrompt = @"
Return ONLY a valid JSON object with no additional text
{
    ""workoutPlan"": {
        ""name"": ""string"",
        ""description"": ""string"",
        ""schedules"": [
            {
                ""dayOfWeek"": ""string"",
                ""workoutPlanName"": ""string"",
                ""isRestDay"": boolean,
                ""workout"": {
                    ""name"": ""string"",
                    ""description"": ""string"",
                    ""exercises"": [
                        {
                            ""exerciseId"": ""string"",
                            ""sets"": number,
                            ""reps"": number,
                            ""order"": number
                        }
                    ]
                }
            }
        ]
    }
}";

        var initialPrompt = $@"You are an advanced Strength and Conditioning Coach. Create a 12-week basketball workout plan structure with the following details:

Overall Structure
  - Create a 12-week workout plan for a basketball athlete.
  - The plan will include strength training days and mobility training days.
  - Strength training frequency is determined by the user's ""# of strength training days"" (based on provided context).
  - Mobility training occurs on all non-strength training days of the week.

Strength Training Day Guidelines
  - Each strength training session consists of 8 exercises in the following order:
    - Plyometric Exercises (3 total if the athlete's main struggle is Vertical Jump; otherwise 2 plyometric exercises)
    - Resistance & Machine Strength Training in this exact sequence:
      - Lower body compound exercise
      - Upper body compound exercise
      - Lower body antagonist movement
      - Upper body antagonist movement
    - Minimum of 1 Accessory Exercises (shoulders, biceps, triceps, calves, glute medius or hip flexors) & Minimum of 1 Core Exercises at the end
  - Every week, apply progressive overload in reps, sets, or by modifying/replacing 1-3 exercises with a more advanced progression.
  - Exercise selection must be influenced by the athlete's Main Struggle (e.g., adding more unilateral movements for Speed).

Mobility Training Day Guidelines
  - Each mobility session consists of 8 exercises.
  - 4 total mobility sessions will be created that consist of 8 different exercises.
  - If the athlete has previous injuries (based on their onboarding answers):
    - The first 4 exercises focus on rehabilitation or prevention for those specific injuries.
    - The last 4 exercises include:
      - One hip mobility exercise
      - One thoracic spine mobility exercise
      - One shoulder mobility exercise
      - One ankle mobility exercise
  - If no previous injury is recorded:
    - The first 4 exercises target the quads, hamstrings, glutes, and adductors (one each).
    - The last 4 exercises remain:
      - One hip mobility exercise
      - One thoracic spine mobility exercise
      - One shoulder mobility exercise
      - One ankle mobility exercise

Player Profile:
- Position: {WorkoutContextHelpers.GetPositionContext(playerSport.Position)}
- Training Days: {WorkoutContextHelpers.GetStrengthTrainingDaysContext(playerProfile.TrainingFrequency)}
- Gender: {WorkoutContextHelpers.GetGenderContext(playerProfile.Gender)}
- Age Group: {WorkoutContextHelpers.GetAgeRangeContext(playerProfile.AgeGroup)}
- Height: {WorkoutContextHelpers.GetHeightContext(playerProfile.Height)}
- Goals: {WorkoutContextHelpers.GetGoalsContext(playerSport.Goals ?? [])}
- Gym Experience: {WorkoutContextHelpers.GetGymExperienceContext(playerProfile.GymExperience)}
- Gym Access: {WorkoutContextHelpers.GetGymAccessContext(playerProfile.GymAccess)}
- Season: {WorkoutContextHelpers.GetSeasonContext(playerSport.SeasonStart, playerSport.SeasonEnd)}
- Injuries: {WorkoutContextHelpers.GetInjuryContext(playerProfile.Injuries)}

The plan should account for the player's position, goals, and training frequency while respecting their experience level and gym access.

Create the detailed workout schedules for Week 1 following on the guidelines provided";

        try
        {
            var response = await _anthropic.Messages.MessagesPostAsync(new CreateMessageParams
            {
                Model = "claude-3-5-sonnet-latest",
                MaxTokens = 8000,
                Temperature = 0.7,
                System = new List<RequestTextBlock>
                {
                    new RequestTextBlock { Text = "You are a professional strength coach. Always return valid JSON matching the specified schema. Never include explanatory text." },
                    new RequestTextBlock { Text = exerciseDatabasePrompt },
                    new RequestTextBlock { Text = schemaPrompt }
                },
                Messages = new List<InputMessage>
                {
                    new InputMessage { Role = InputMessageRole.User, Content = initialPrompt }
                }
            });

            if (response.StopReason == MessageStopReason.MaxTokens)
            {
                throw new Exception("Response was truncated due to token limit. Please reduce the request size or increase max_tokens.");
            }

            var content = response.Content[0].Text.Text.ToString();
            _logger.LogInformation("Received response from Anthropic: {Content}", content);

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true
                };

                var workoutPlan = JsonSerializer.Deserialize<ResponseWorkoutPlanResponse>(content, options);

                if (workoutPlan == null)
                {
                    _logger.LogError("Deserialization resulted in null object. Raw content: {Content}", content);
                    throw new Exception("Failed to deserialize workout plan");
                }

                _logger.LogInformation("Successfully deserialized workout plan with name: {Name}", workoutPlan.WorkoutPlan.Name);

                await SaveWorkoutPlanToDatabase(workoutPlan, id);

                _logger.LogInformation("Workout plan generated successfully for user {UserId}", userId);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error. Raw content: {Content}", content);
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating workout plan for user {UserId}", userId);
            throw;
        }
    }

    private async Task SaveWorkoutPlanToDatabase(ResponseWorkoutPlanResponse workoutPlan, Guid userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _logger.LogInformation("Starting to save workout plan for user {UserId}", userId);

            var today = DateTime.UtcNow;
            var startDate = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, DateTimeKind.Utc);

            // Find the next Monday
            var currentWeekDay = today.DayOfWeek;
            startDate = currentWeekDay == DayOfWeek.Sunday ? startDate.AddDays(1) : startDate.AddDays(-(int)currentWeekDay);

            // Check if we're more than halfway through the current week
            var isMoreThanHalfwayThroughWeek = (int)currentWeekDay >= 4; // Thursday or later

            // Determine if we need a 2-week plan
            var hasActivePlan = await HasActivePlan(userId);
            var isMoreThanHalfwayThroughPlan = await IsMoreThanHalfwayThroughPlan(userId);

            // Generate a 2-week plan if:
            // 1. User has no active plan AND we're more than halfway through the week, OR
            // 2. User has an active plan AND is more than halfway through it
            var needsTwoWeekPlan = (!hasActivePlan && isMoreThanHalfwayThroughWeek) ||
                                 (hasActivePlan && isMoreThanHalfwayThroughPlan);

            var planDuration = needsTwoWeekPlan ? 14 : 7;
            var endDate = startDate.AddDays(planDuration);

            _logger.LogInformation("Creating workout plan with start date {StartDate} and end date {EndDate}", startDate, endDate);

            // Create the workout plan
            var createdPlan = new WorkoutPlan
            {
                UserId = userId,
                Name = workoutPlan.WorkoutPlan.Name,
                Description = workoutPlan.WorkoutPlan.Description,
                StartDate = startDate,
                EndDate = endDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.WorkoutPlans.Add(createdPlan);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created workout plan with ID {PlanId}", createdPlan.Id);

            // Get all exercise IDs from the database for validation
            var validExerciseIds = await _context.Exercises
                .Select(e => e.Id)
                .ToListAsync();
            _logger.LogInformation("Retrieved {Count} valid exercise IDs from database: {ExerciseIds}",
                validExerciseIds.Count,
                string.Join(", ", validExerciseIds));

            // Create schedules for each workout
            foreach (var schedule in workoutPlan.WorkoutPlan.Schedules)
            {
                // Calculate the date for this schedule based on the day of week
                var scheduleDate = CalculateScheduleDate(startDate, schedule.DayOfWeek);

                _logger.LogInformation("Creating schedule for {DayOfWeek} on {ScheduleDate}", schedule.DayOfWeek, scheduleDate);

                // Create the schedule
                var createdSchedule = new WorkoutPlanSchedule
                {
                    WorkoutPlanId = createdPlan.Id,
                    DayOfWeek = schedule.DayOfWeek,
                    WorkoutPlanName = schedule.WorkoutPlanName,
                    Date = scheduleDate
                };

                _context.WorkoutSchedules.Add(createdSchedule);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created schedule with ID {ScheduleId}", createdSchedule.Id);

                // If it's not a rest day and has a workout, create the workout
                if (!schedule.IsRestDay && schedule.Workout != null)
                {
                    _logger.LogInformation("Creating workout for schedule {ScheduleId}", createdSchedule.Id);

                    // Create the workout
                    var createdWorkout = new Workout
                    {
                        Name = schedule.Workout.Name,
                        Description = schedule.Workout.Description,
                        WorkoutPlanScheduleId = createdSchedule.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    _context.Workouts.Add(createdWorkout);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Created workout with ID {WorkoutId}", createdWorkout.Id);

                    // Create exercises for each workout
                    foreach (var exercise in schedule.Workout.Exercises)
                    {
                        _logger.LogInformation("Processing exercise with ID {ExerciseId} for workout {WorkoutId}",
                            exercise.ExerciseId,
                            createdWorkout.Id);

                        // Validate that the exercise ID exists
                        if (!validExerciseIds.Contains(exercise.ExerciseId))
                        {
                            _logger.LogError("Invalid exercise ID {ExerciseId} found in workout plan. Valid exercise IDs are: {ValidExerciseIds}",
                                exercise.ExerciseId,
                                string.Join(", ", validExerciseIds));
                            throw new InvalidOperationException($"Invalid exercise ID {exercise.ExerciseId} found in workout plan");
                        }

                        try
                        {
                            var workoutExercise = new WorkoutExercise
                            {
                                WorkoutId = createdWorkout.Id,
                                ExerciseId = exercise.ExerciseId,
                                Sets = exercise.Sets,
                                Reps = exercise.Reps,
                                Order = exercise.Order
                            };

                            _context.WorkoutExercises.Add(workoutExercise);
                            await _context.SaveChangesAsync();
                            _logger.LogInformation("Successfully saved exercise {ExerciseId} for workout {WorkoutId}",
                                exercise.ExerciseId,
                                createdWorkout.Id);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to save exercise {ExerciseId} for workout {WorkoutId}. Exercise details: Sets={Sets}, Reps={Reps}, Order={Order}",
                                exercise.ExerciseId,
                                createdWorkout.Id,
                                exercise.Sets,
                                exercise.Reps,
                                exercise.Order);
                            throw;
                        }
                    }
                }
            }

            // If this is a 2-week plan, duplicate the schedules for the second week
            if (needsTwoWeekPlan)
            {
                _logger.LogInformation("Creating second week of workout plan");
                var secondWeekStartDate = startDate.AddDays(7);

                foreach (var schedule in workoutPlan.WorkoutPlan.Schedules)
                {
                    // Calculate the date for this schedule based on the day of week
                    var scheduleDate = CalculateScheduleDate(secondWeekStartDate, schedule.DayOfWeek);

                    _logger.LogInformation("Creating second week schedule for {DayOfWeek} on {ScheduleDate}", schedule.DayOfWeek, scheduleDate);

                    // Create the schedule for the second week
                    var createdSchedule = new WorkoutPlanSchedule
                    {
                        WorkoutPlanId = createdPlan.Id,
                        DayOfWeek = schedule.DayOfWeek,
                        WorkoutPlanName = schedule.WorkoutPlanName,
                        Date = scheduleDate
                    };

                    _context.WorkoutSchedules.Add(createdSchedule);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Created second week schedule with ID {ScheduleId}", createdSchedule.Id);

                    // If it's not a rest day and has a workout, create the workout
                    if (!schedule.IsRestDay && schedule.Workout != null)
                    {
                        _logger.LogInformation("Creating second week workout for schedule {ScheduleId}", createdSchedule.Id);

                        // Create the workout
                        var createdWorkout = new Workout
                        {
                            Name = schedule.Workout.Name,
                            Description = schedule.Workout.Description,
                            WorkoutPlanScheduleId = createdSchedule.Id,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        _context.Workouts.Add(createdWorkout);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Created second week workout with ID {WorkoutId}", createdWorkout.Id);

                        // Create exercises for each workout
                        foreach (var exercise in schedule.Workout.Exercises)
                        {
                            _logger.LogInformation("Processing second week exercise with ID {ExerciseId} for workout {WorkoutId}",
                                exercise.ExerciseId,
                                createdWorkout.Id);

                            // Validate that the exercise ID exists
                            if (!validExerciseIds.Contains(exercise.ExerciseId))
                            {
                                _logger.LogError("Invalid exercise ID {ExerciseId} found in second week workout plan. Valid exercise IDs are: {ValidExerciseIds}",
                                    exercise.ExerciseId,
                                    string.Join(", ", validExerciseIds));
                                throw new InvalidOperationException($"Invalid exercise ID {exercise.ExerciseId} found in second week workout plan");
                            }

                            try
                            {
                                var workoutExercise = new WorkoutExercise
                                {
                                    WorkoutId = createdWorkout.Id,
                                    ExerciseId = exercise.ExerciseId,
                                    Sets = exercise.Sets,
                                    Reps = exercise.Reps,
                                    Order = exercise.Order
                                };

                                _context.WorkoutExercises.Add(workoutExercise);
                                await _context.SaveChangesAsync();
                                _logger.LogInformation("Successfully saved second week exercise {ExerciseId} for workout {WorkoutId}",
                                    exercise.ExerciseId,
                                    createdWorkout.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Failed to save second week exercise {ExerciseId} for workout {WorkoutId}. Exercise details: Sets={Sets}, Reps={Reps}, Order={Order}",
                                    exercise.ExerciseId,
                                    createdWorkout.Id,
                                    exercise.Sets,
                                    exercise.Reps,
                                    exercise.Order);
                                throw;
                            }
                        }
                    }
                }
            }

            // If we get here, everything was successful, so commit the transaction
            await transaction.CommitAsync();
            _logger.LogInformation("Successfully completed saving workout plan for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            // If any error occurs, roll back the transaction
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error saving workout plan to database for user {UserId}. All changes have been rolled back.", userId);
            throw;
        }
    }

    private DateTime CalculateScheduleDate(DateTime startDate, TrainingDayOfWeek dayOfWeek)
    {
        var scheduleDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0, DateTimeKind.Utc);
        var targetDay = ((int)dayOfWeek);
        var currentDay = (int)scheduleDate.DayOfWeek;

        // Calculate days to add to reach the target day
        var daysToAdd = targetDay - currentDay;
        if (daysToAdd < 0)
        {
            daysToAdd += 7; // Wrap to next week if target day is earlier in the week
        }

        return scheduleDate.AddDays(daysToAdd);
    }

    private async Task<bool> HasActivePlan(Guid userId)
    {
        var activePlan = await GetCurrentActivePlan(userId);
        if (activePlan is null) return false;

        var now = DateTime.UtcNow;
        return now >= activePlan?.StartDate && now <= activePlan?.EndDate;
    }

    private async Task<bool> IsMoreThanHalfwayThroughPlan(Guid userId)
    {
        var activePlan = await GetCurrentActivePlan(userId);
        if (activePlan is null) return false;

        var now = DateTime.UtcNow;
        var totalDuration = (activePlan?.EndDate - activePlan?.StartDate)?.TotalDays;
        var elapsedDuration = (now - activePlan?.StartDate)?.TotalDays;

        return elapsedDuration > totalDuration / 2;
    }

    private async Task<WorkoutPlan?> GetCurrentActivePlan(Guid userId)
    {
        var plans = await _context.WorkoutPlans
            .Where(p => p.UserId == userId)
            .ToListAsync();

        var now = DateTime.UtcNow;
        return plans.FirstOrDefault(p => p.StartDate <= now && p.EndDate >= now);
    }
}

public class ResponseWorkoutPlanResponse
{
    [JsonPropertyName("workoutPlan")]
    public ResponseWorkoutPlan WorkoutPlan { get; set; } = null!;
}

public class ResponseWorkoutPlan
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("schedules")]
    public List<ResponseSchedule> Schedules { get; set; } = new();
}

public class ResponseSchedule
{
    [JsonPropertyName("dayOfWeek")]
    public TrainingDayOfWeek DayOfWeek { get; set; }

    [JsonPropertyName("workoutPlanName")]
    public string WorkoutPlanName { get; set; } = null!;

    [JsonPropertyName("isRestDay")]
    public bool IsRestDay { get; set; }

    [JsonPropertyName("workout")]
    public ResponseWorkout? Workout { get; set; }
}

public class ResponseWorkout
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    [JsonPropertyName("exercises")]
    public List<ResponseExercise> Exercises { get; set; } = new();
}

public class ResponseExercise
{
    [JsonPropertyName("exerciseId")]
    public Guid ExerciseId { get; set; }

    [JsonPropertyName("sets")]
    public int Sets { get; set; }

    [JsonPropertyName("reps")]
    public int Reps { get; set; }

    [JsonPropertyName("order")]
    public int Order { get; set; }
}