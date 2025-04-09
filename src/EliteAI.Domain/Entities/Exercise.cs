using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteAI.Domain.Entities;

[Table("exercises")]
public class Exercise
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("cover_image_url")]
    public string? CoverImageUrl { get; set; }

    [Column("video_url")]
    public string? VideoUrl { get; set; }

    [Column("equipment_needed")]
    public string[] EquipmentNeeded { get; set; } = Array.Empty<string>();

    [Column("muscle_group")]
    public MuscleGroup[] MuscleGroup { get; set; } = Array.Empty<MuscleGroup>();

    [Column("level")]
    public ExerciseLevel Level { get; set; }

    [Column("type")]
    public ExerciseType Type { get; set; }

    public virtual ICollection<PlayerWorkoutExercise> PlayerWorkoutExercises { get; set; } = new List<PlayerWorkoutExercise>();
} 