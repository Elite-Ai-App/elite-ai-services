using System.ComponentModel.DataAnnotations;
using EliteAI.Application.DTOs.Base;
using EliteAI.Domain;

namespace EliteAI.Application.DTOs.Workout;

public class WorkoutDto : BaseDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    public WorkoutScheduleDto[] Schedules { get; set; } = Array.Empty<WorkoutScheduleDto>();
}

public class WorkoutScheduleDto : BaseDto
{
    [Required]
    public Guid WorkoutId { get; set; }

    [Required]
    public DayOfWeek DayOfWeek { get; set; }

    [Required]
    [StringLength(100)]
    public string WorkoutPlanName { get; set; } = string.Empty;

    public bool IsRestDay { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    public WorkoutExerciseDto[] Exercises { get; set; } = Array.Empty<WorkoutExerciseDto>();
}

public class WorkoutExerciseDto : BaseDto
{
    [Required]
    public Guid WorkoutId { get; set; }

    [Required]
    public Guid ExerciseId { get; set; }

    [Required]
    [Range(1, 20)]
    public int Sets { get; set; }

    [Required]
    [Range(1, 100)]
    public int Reps { get; set; }

    [Required]
    [Range(1, 100)]
    public int Order { get; set; }
}

public class CreateWorkoutDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndDate { get; set; }

    public CreateWorkoutScheduleDto[] Schedules { get; set; } = Array.Empty<CreateWorkoutScheduleDto>();
}

public class CreateWorkoutScheduleDto
{
    [Required]
    public DayOfWeek DayOfWeek { get; set; }

    [Required]
    [StringLength(100)]
    public string WorkoutPlanName { get; set; } = string.Empty;

    public bool IsRestDay { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    public CreateWorkoutExerciseDto[] Exercises { get; set; } = Array.Empty<CreateWorkoutExerciseDto>();
}

public class CreateWorkoutExerciseDto
{
    [Required]
    public Guid ExerciseId { get; set; }

    [Required]
    [Range(1, 20)]
    public int Sets { get; set; }

    [Required]
    [Range(1, 100)]
    public int Reps { get; set; }

    [Required]
    [Range(1, 100)]
    public int Order { get; set; }
}

public class UpdateWorkoutDto
{
    [StringLength(100)]
    public string? Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? EndDate { get; set; }

    public UpdateWorkoutScheduleDto[]? Schedules { get; set; }
}

public class UpdateWorkoutScheduleDto
{
    public DayOfWeek? DayOfWeek { get; set; }

    [StringLength(100)]
    public string? WorkoutPlanName { get; set; }

    public bool? IsRestDay { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? Date { get; set; }

    public UpdateWorkoutExerciseDto[]? Exercises { get; set; }
}

public class UpdateWorkoutExerciseDto
{
    public Guid? ExerciseId { get; set; }

    [Range(1, 20)]
    public int? Sets { get; set; }

    [Range(1, 100)]
    public int? Reps { get; set; }

    [Range(1, 100)]
    public int? Order { get; set; }
} 