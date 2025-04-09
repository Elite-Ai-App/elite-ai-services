using System.ComponentModel.DataAnnotations;
using EliteAI.Application.DTOs.Base;
using EliteAI.Domain;

namespace EliteAI.Application.DTOs.PlayerProfile;

public class PlayerProfileDto : BaseDto
{
    [Required]
    public Guid UserId { get; set; }

    [Range(0, 300)]
    public int? Height { get; set; }

    [Range(0, 500)]
    public int? Weight { get; set; }

    public Gender? Gender { get; set; }

    public AgeGroup? AgeGroup { get; set; }

    public GymExperience? GymExperience { get; set; }

    public GymAccess? GymAccess { get; set; }

    public string[] AvailableEquipment { get; set; } = Array.Empty<string>();

    [Required]
    public UnitType UnitType { get; set; }

    public bool Injured { get; set; }

    public InjuryArea[] Injuries { get; set; } = Array.Empty<InjuryArea>();

    public TrainingFrequency? TrainingFrequency { get; set; }
}

public class CreatePlayerProfileDto
{
    [Required]
    public Guid UserId { get; set; }

    [Range(0, 300)]
    public int? Height { get; set; }

    [Range(0, 500)]
    public int? Weight { get; set; }

    public Gender? Gender { get; set; }

    public AgeGroup? AgeGroup { get; set; }

    public GymExperience? GymExperience { get; set; }

    public GymAccess? GymAccess { get; set; }

    public string[] AvailableEquipment { get; set; } = Array.Empty<string>();

    [Required]
    public UnitType UnitType { get; set; }

    public bool Injured { get; set; }

    public InjuryArea[] Injuries { get; set; } = Array.Empty<InjuryArea>();

    public TrainingFrequency? TrainingFrequency { get; set; }
}

public class UpdatePlayerProfileDto
{
    [Range(0, 300)]
    public int? Height { get; set; }

    [Range(0, 500)]
    public int? Weight { get; set; }

    public Gender? Gender { get; set; }

    public AgeGroup? AgeGroup { get; set; }

    public GymExperience? GymExperience { get; set; }

    public GymAccess? GymAccess { get; set; }

    public string[]? AvailableEquipment { get; set; }

    public UnitType? UnitType { get; set; }

    public bool? Injured { get; set; }

    public InjuryArea[]? Injuries { get; set; }

    public TrainingFrequency? TrainingFrequency { get; set; }
} 