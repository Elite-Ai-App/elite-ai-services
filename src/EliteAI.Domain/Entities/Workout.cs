using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteAI.Domain.Entities;

[Table("player_workouts")]
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

    [Column("player_workout_plan_schedule_id")]
    public Guid WorkoutPlanScheduleId { get; set; }

    [ForeignKey("WorkoutPlanScheduleId")]
    public virtual WorkoutPlanSchedule WorkoutPlanSchedule { get; set; } = null!;

    public virtual ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
    public virtual WorkoutLog? WorkoutLog { get; set; }
} 