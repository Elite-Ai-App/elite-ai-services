using EliteAI.Domain.Enums;

namespace EliteAI.Domain.Helpers;

public static class WorkoutContextHelpers
{
    public static string GetAgeRangeContext(AgeGroup? ageGroup)
    {
        return ageGroup switch
        {
            AgeGroup.JUNIOR => @"
- Basic plyometrics, bodyweight exercises, isometrics, static core",
            AgeGroup.TEEN => @"
- Basic/intermediate plyometrics, basic upper/lower body resistance, machine exercises, isometrics, static core",
            AgeGroup.YOUNG_ADULT => @"
- Minimum of 1 Power/reactive exercises, intermediate/advanced plyometrics, intermediate/advanced resistance, weighted isometrics, functional core",
            AgeGroup.ADULT => @"
- Minimum of 1 Power/reactive exercises (cleans, high pulls, snatches), basic/intermediate/advanced plyometrics, basic/intermediate/advanced resistance, minimum of 1 weighted isometrics, functional core",
            _ => @"
- Minimum of 1 Power/reactive exercises, intermediate/advanced plyometrics, intermediate/advanced resistance, weighted isometrics, functional core"
        };
    }

    public static string GetGenderContext(Gender? gender)
    {
        return gender switch
        {
            Gender.MALE => "",
            Gender.FEMALE => @"
- Male: No specific change
- Female: 
  • Emphasize glute strengthening
  • Isometric hip/knee holds
  • ACL prevention in mobility and strength training
  • Minimum one relevant exercise for both mobility and strength training
    (e.g., Nordic curls, single leg balance)",
            _ => ""
        };
    }

    public static string GetGymExperienceContext(GymExperience? gymExperience)
    {
        return gymExperience switch
        {
            GymExperience.BEGINNER => @"
- Only include beginner exercises",
            GymExperience.INTERMEDIATE => @"
- Intermediates + beginner exercises",
            GymExperience.ADVANCED => @"
- Include Beginner, intermediate, and advanced exercises
- Mainly include Advanced and intermediate",
            _ => @"
- Intermediates + beginner exercises"
        };
    }

    public static string GetGymAccessContext(GymAccess? gymAccess)
    {
        return gymAccess switch
        {
            GymAccess.FULL_ACCESS => @"
- Full Gym Access: All exercise options available",
            GymAccess.LIMITED_ACCESS => @"
- Limited Gym Access: Only include exercises that match available equipment",
            GymAccess.NO_ACCESS => @"
- Bodyweight Only: Only include bodyweight exercises",
            _ => @"
- Full Gym Access: All exercise options available"
        };
    }

    public static string GetHeightContext(decimal? height)
    {
        if (!height.HasValue)
            return "";

        return height > 160 ? @"
- Avoid back squats and barbell deadlifts in the program" : "";
    }

    public static string GetInjuryContext(InjuryArea[]? injuries)
    {
        if (injuries == null || !injuries.Any())
        {
            return @"
- First 4 exercises target:
  • Quads
  • Hamstrings
  • Glutes
  • Adductors
- Last 4 exercises remain:
  • One hip mobility exercise
  • One thoracic spine mobility exercise
  • One shoulder mobility exercise
  • One ankle mobility exercise";
        }

        return $@"
- First 4 exercises focus on rehabilitation or prevention the following injuries:
  {string.Join(", ", injuries)}
- Last 4 exercises include:
  • One hip mobility exercise
  • One thoracic spine mobility exercise
  • One shoulder mobility exercise
  • One ankle mobility exercise";
    }

    public static string GetTrainingFrequencyContext(TrainingFrequency? trainingFrequency)
    {
        return trainingFrequency switch
        {
            TrainingFrequency.ONE_DAY_PER_WEEK => @"
1 Day/Week:
- Full-body strength on Monday + additional isometrics",
            TrainingFrequency.TWO_DAYS_PER_WEEK => @"
2 Days/Week:
- Full-body strength on Monday & Thursday",
            TrainingFrequency.THREE_DAYS_PER_WEEK => @"
3 Days/Week:
- Full-body strength on Monday, Wednesday & Friday",
            TrainingFrequency.FOUR_DAYS_PER_WEEK => @"
4 Days/Week:
- Full-body strength on Monday, Tuesday, Thursday & Friday",
            TrainingFrequency.FIVE_DAYS_PER_WEEK => @"
5 Days/Week:
- Full-body strength on Monday, Tuesday, Wednesday, Thursday & Friday",
            TrainingFrequency.SIX_DAYS_PER_WEEK => @"
6 Days/Week:
- Full-body strength on Monday, Tuesday, Wednesday, Thursday, Friday & Saturday",
            _ => @"
4 Days/Week:
- Full-body strength on Monday, Tuesday, Thursday & Friday"
        };
    }

    public static string GetStrengthTrainingDaysContext(TrainingFrequency? trainingFrequency)
    {
        return trainingFrequency switch
        {
            TrainingFrequency.ONE_DAY_PER_WEEK => "1",
            TrainingFrequency.TWO_DAYS_PER_WEEK => "2",
            TrainingFrequency.THREE_DAYS_PER_WEEK => "3",
            TrainingFrequency.FOUR_DAYS_PER_WEEK => "4",
            TrainingFrequency.FIVE_DAYS_PER_WEEK => "5",
            TrainingFrequency.SIX_DAYS_PER_WEEK => "6",
            _ => "4"
        };
    }

    public static string GetGoalsContext(Goal[]? goals)
    {
        if (goals == null || !goals.Any())
            return "";

        var goalContexts = new List<string>();

        foreach (var goal in goals)
        {
            switch (goal)
            {
                case Goal.CARDIO:
                    goalContexts.Add(@"
Cardio:
- Increase length of isometric holds
- Minimum 2 isometric holds per strength training workout");
                    break;
                case Goal.CORE:
                    goalContexts.Add(@"
Core Strength:
- Increase sets, reps, and time for core exercises
- Minimum 2 core strength training exercises");
                    break;
                case Goal.VERTICAL_JUMP:
                    goalContexts.Add(@"
Vertical Jump:
- Place 3 plyometric exercises at the start of each strength session");
                    break;
                case Goal.SPEED:
                    goalContexts.Add(@"
Speed:
- Increase unilateral resistance exercises
- Minimum 2 unilateral strength training exercises per workout");
                    break;
                case Goal.LATERAL_QUICKNESS:
                    goalContexts.Add(@"
Lateral Quickness:
- Increase lateral resistance exercises
- Minimum 1 lateral focused strength training exercise per workout");
                    break;
            }
        }

        return string.Join("\n\n", goalContexts);
    }

    public static string GetPositionContext(Position? position)
    {
        return position switch
        {
            Position.POINT_GUARD => @"
- Point Guard:
  - Single Leg RDL
  - Split Squats or Bulgarian Splits Squats
  - Pallof Press",
            Position.SHOOTING_GUARD => @"
- Shooting Guard:
  - Trap bar Deadlift jumps or Dumbbell Jumps
  - Cable Woodchopper
  - Lateral bounds",
            Position.SMALL_FORWARD => @"
- Small Forward:
  - Front Squat or Goblet Squat
  - Pull Ups
  - Single Leg Step Ups",
            Position.POWER_FORWARD => @"
- Power Forward:
  - Trap Bar Deadlift
  - Nordic hamstring curls
  - Copenhagen Side plank",
            Position.CENTER => @"
- Center:
  - Hip Thruster
  - Overhead press
  - Farmer's walk",
            _ => ""
        };
    }

    public static string GetSeasonContext(DateTime? seasonStart, DateTime? seasonEnd)
    {
        if (!seasonStart.HasValue || !seasonEnd.HasValue)
            return "Off-Season: range 4–6 sets, 6–12 reps per exercise";

        var now = DateTime.UtcNow;
        var isInSeason = now >= seasonStart && now <= seasonEnd;

        if (isInSeason)
        {
            var twelveWeeksFromNow = now.AddDays(84); // 12 weeks * 7 days

            if (twelveWeeksFromNow > seasonEnd)
            {
                var weeksUntilSeasonEnd = Math.Ceiling((seasonEnd.Value - now).TotalDays / 7);
                var remainingWeeks = 12 - weeksUntilSeasonEnd;

                return $@"First {weeksUntilSeasonEnd} weeks:
- In-Season: range 3–4 sets, 6–10 reps per exercise

Remaining {remainingWeeks} weeks:
- Off-Season: range 4–6 sets, 6–12 reps per exercise";
            }

            return "In-Season: range 3–4 sets, 6–10 reps per exercise";
        }

        return "Off-Season: range 4–6 sets, 6–12 reps per exercise";
    }
} 