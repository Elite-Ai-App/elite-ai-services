using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteAI.Domain.Entities;

[Table("workout_logs")]
public class WorkoutLog
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("profile_id")]
    public Guid ProfileId { get; set; }

    [ForeignKey("profile")]
    public virtual Profile Profile { get; set; } = null!;

    [Column("workout_id")]
    public Guid WorkoutId { get; set; }

    [ForeignKey("workout")]
    public virtual Workout Workout { get; set; } = null!;

    [Column("started_at")]

    public DateTime StartedAt { get; set; }

    [Column("completed_at")]
    public DateTime CompletedAt { get; set; }

    [Column("volume")]

    public decimal? Volume { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Column("rating")]
    public int? Rating { get; set; }

    [Column("energy_level")]
    public int? EnergeyLevel { get; set; }

    [Column("mood")]
    public int? Mood { get; set; }
       
} 