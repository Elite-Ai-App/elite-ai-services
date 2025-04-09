using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteAI.Domain.Entities;

public class User
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("base_username")]
    public string? BaseUsername { get; set; }

    [Column("numeric_suffix")]
    public string? NumericSuffix { get; set; }

    [Column("user_name")]
    public string? UserName { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("unit_type")]
    public UnitType UnitType { get; set; } = UnitType.IMPERIAL;

    [Column("profile_picture_url")]
    public string? ProfilePictureUrl { get; set; }

    [Column("onboarding_complete")]
    public bool OnboardingComplete { get; set; } = false;

    public virtual PlayerProfile? PlayerProfile { get; set; }
    public virtual ICollection<PlayerWorkoutPlan> PlayerWorkoutPlanSchedules { get; set; } = new List<PlayerWorkoutPlan>();
} 