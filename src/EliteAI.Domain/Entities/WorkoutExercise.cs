using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteAI.Domain.Entities;

[Table("player_workout_exercises")]
public class WorkoutExercise
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("player_workout_id")]
    public Guid WorkoutId { get; set; }

    [ForeignKey("WorkoutId")]
    public virtual Workout Workout { get; set; } = null!;

    [Column("exercise_id")]
    public Guid ExerciseId { get; set; }

    [ForeignKey("ExerciseId")]
    public virtual Exercise Exercise { get; set; } = null!;

    [Column("sets")]
    public int Sets { get; set; }

    [Column("reps")]
    public int Reps { get; set; }

    [Column("order")]
    public int Order { get; set; }
} 