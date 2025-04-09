using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteAI.Domain.Enums;

namespace EliteAI.Domain.Entities;

[Table("sports")]
public class Sports
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("profile_id")]
    public Guid ProfileId { get; set; }

    [ForeignKey("profile")]
    public virtual Profile Profile { get; set; } = null!;

    [Column("sport")]
    public Sport Sport { get; set; }

    [Column("season_start")]
    public DateTime? SeasonStart { get; set; }

    [Column("season_end")]
    public DateTime? SeasonEnd { get; set; }

    [Column("position")]
    public Position? Position { get; set; }

    [Column("sport_level")]
    public SportLevel? SportLevel { get; set; }

    [Column("sports_goal")]
    public Goal[] Goals { get; set; } = Array.Empty<Goal>();
} 