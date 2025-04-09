using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteAI.Domain.Entities;

[Table("workouts")]
public class Workout
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("workout_plan_schedule_id")]
    public Guid WorkoutPlanScheduleId { get; set; }

    [ForeignKey("WorkoutPlanScheduleId")]
    public virtual WorkoutPlanSchedule WorkoutPlanSchedule { get; set; } = null!;

    [Column("exercises")]
    public virtual ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();

    [Column("workout_log")]
    public virtual WorkoutLog? WorkoutLog { get; set; }
} 