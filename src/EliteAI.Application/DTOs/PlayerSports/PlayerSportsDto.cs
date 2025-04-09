using System.ComponentModel.DataAnnotations;
using EliteAI.Application.DTOs.Base;
using EliteAI.Domain;

namespace EliteAI.Application.DTOs.PlayerSports;

public class PlayerSportsDto : BaseDto
{
    [Required]
    public Guid PlayerProfileId { get; set; }

    [Required]
    public Sport Sport { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? SeasonStart { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? SeasonEnd { get; set; }

    public Position? Position { get; set; }

    public SportLevel? SportLevel { get; set; }

    public Goal[] Goals { get; set; } = Array.Empty<Goal>();
}

public class CreatePlayerSportsDto
{
    [Required]
    public Guid PlayerProfileId { get; set; }

    [Required]
    public Sport Sport { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? SeasonStart { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? SeasonEnd { get; set; }

    public Position? Position { get; set; }

    public SportLevel? SportLevel { get; set; }

    public Goal[] Goals { get; set; } = Array.Empty<Goal>();
}

public class UpdatePlayerSportsDto
{
    [DataType(DataType.DateTime)]
    public DateTime? SeasonStart { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? SeasonEnd { get; set; }

    public Position? Position { get; set; }

    public SportLevel? SportLevel { get; set; }

    public Goal[]? Goals { get; set; }
} 