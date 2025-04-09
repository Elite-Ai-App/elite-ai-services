using System.Text.Json;
using Anthropic;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;
using EliteAI.Domain.Helpers;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        var strengthExercises = await _context.Exercises
            .Where(e => e.Type == ExerciseType.STRENGTH)
            .ToListAsync();

        var mobilityExercises = await _context.Exercises
            .Where(e => e.Type == ExerciseType.MOBILITY)
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

            var content = response.Content[0].Text;
            var workoutPlan = JsonSerializer.Deserialize<ResponseWorkoutPlanResponse>(content.ToJson());

            await SaveWorkoutPlanToDatabase(workoutPlan!, id);

            _logger.LogInformation("Workout plan generated successfully for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating workout plan for user {UserId}", userId);
            throw;
        }
    }

    private async Task SaveWorkoutPlanToDatabase(ResponseWorkoutPlanResponse workoutPlan, Guid userId)
    {
        var today = DateTime.UtcNow;
        var startDate = new DateTime(today.Year, today.Month, today.Day);

        // Find the next Monday
        var currentDay = startDate.DayOfWeek;
        var daysUntilMonday = currentDay == DayOfWeek.Sunday ? 1 : (8 - (int)currentDay) % 7;
        startDate = startDate.AddDays(daysUntilMonday);

        // Check if we're more than halfway through the current week
        var currentWeekDay = today.DayOfWeek;
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

        // Create the workout plan
        var createdPlan = new WorkoutPlan
        {
            UserId = userId,
            Name = workoutPlan.WorkoutPlan.Name,
            Description = workoutPlan.WorkoutPlan.Description,
            StartDate = startDate,
            EndDate = endDate
        };

        _context.WorkoutPlans.Add(createdPlan);
        await _context.SaveChangesAsync();

        // Create schedules for each workout
        foreach (var schedule in workoutPlan.WorkoutPlan.Schedules)
        {
            // Calculate the date for this schedule based on the day of week
            var scheduleDate = CalculateScheduleDate(startDate, schedule.DayOfWeek);

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

            // If it's not a rest day and has a workout, create the workout
            if (!schedule.IsRestDay && schedule.Workout != null)
            {
                // Create the workout
                var createdWorkout = new Workout
                {
                    Name = schedule.Workout.Name,
                    Description = schedule.Workout.Description,
                    WorkoutPlanScheduleId = createdSchedule.Id
                };

                _context.Workouts.Add(createdWorkout);
                await _context.SaveChangesAsync();

                // Create exercises for each workout
                foreach (var exercise in schedule.Workout.Exercises)
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
                }
            }
        }

        await _context.SaveChangesAsync();

        // If this is a 2-week plan, duplicate the schedules for the second week
        if (needsTwoWeekPlan)
        {
            var secondWeekStartDate = startDate.AddDays(7);

            foreach (var schedule in workoutPlan.WorkoutPlan.Schedules)
            {
                // Calculate the date for this schedule based on the day of week
                var scheduleDate = CalculateScheduleDate(secondWeekStartDate, schedule.DayOfWeek);

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

                // If it's not a rest day and has a workout, create the workout
                if (!schedule.IsRestDay && schedule.Workout != null)
                {
                    // Create the workout
                    var createdWorkout = new Workout
                    {
                        Name = schedule.Workout.Name,
                        Description = schedule.Workout.Description,
                        WorkoutPlanScheduleId = createdSchedule.Id
                    };

                    _context.Workouts.Add(createdWorkout);
                    await _context.SaveChangesAsync();

                    // Create exercises for each workout
                    foreach (var exercise in schedule.Workout.Exercises)
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
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }

    private DateTime CalculateScheduleDate(DateTime startDate, TrainingDayOfWeek dayOfWeek)
    {
        var scheduleDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
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
    public ResponseWorkoutPlan WorkoutPlan { get; set; } = null!;
}

public class ResponseWorkoutPlan
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ResponseSchedule> Schedules { get; set; } = new();
}

public class ResponseSchedule
{
    public TrainingDayOfWeek DayOfWeek { get; set; }
    public string WorkoutPlanName { get; set; } = null!;
    public bool IsRestDay { get; set; }
    public ResponseWorkout? Workout { get; set; }
}

public class ResponseWorkout
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ResponseExercise> Exercises { get; set; } = new();
}

public class ResponseExercise
{
    public Guid ExerciseId { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int Order { get; set; }
} 