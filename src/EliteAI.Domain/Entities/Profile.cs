using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteAI.Domain.Enums;

namespace EliteAI.Domain.Entities;

[Table("player_profiles")]
public class Profile
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [Column("height")]
    public decimal? Height { get; set; }

    [Column("weight")]
    public decimal? Weight { get; set; }

    [Column("gender")]
    public Gender? Gender { get; set; }

    [Column("age_group")]
    public AgeGroup? AgeGroup { get; set; }

    [Column("gym_experience")]
    public GymExperience? GymExperience { get; set; }

    [Column("gym_access")]
    public GymAccess? GymAccess { get; set; }

    [Column("available_equipment")]
    public string[] AvailableEquipment { get; set; } = Array.Empty<string>();

    [Column("unit_type")]
    public UnitType UnitType { get; set; } = UnitType.METRIC;

    [Column("injured")]
    public bool Injured { get; set; } = false;

    [Column("injuries")]
    public InjuryArea[] Injuries { get; set; } = Array.Empty<InjuryArea>();

    [Column("training_frequency")]
    public TrainingFrequency? TrainingFrequency { get; set; }

    public virtual ICollection<Sports> Sports { get; set; } = new List<Sports>();
    public virtual ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
} 