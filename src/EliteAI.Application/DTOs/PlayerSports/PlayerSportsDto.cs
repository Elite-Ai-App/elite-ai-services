using System.ComponentModel.DataAnnotations;
using EliteAI.Application.DTOs.Base;
using EliteAI.Domain;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.DTOs.Sports;

public class SportsDto : BaseDto
{
    [Required]
    public Guid ProfileId { get; set; }

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

public class CreateSportsDto
{
    [Required]
    public Guid ProfileId { get; set; }

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

public class UpdateSportsDto
{
    [DataType(DataType.DateTime)]
    public DateTime? SeasonStart { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? SeasonEnd { get; set; }

    public Position? Position { get; set; }

    public SportLevel? SportLevel { get; set; }

    public Goal[]? Goals { get; set; }
} 