using EliteAI.Domain;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.DTOs.Onboarding
{
    public class CompleteOnboardingDTO
    {
        public UnitType UnitType { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public Gender? Gender { get; set; }
        public AgeGroup? AgeGroup { get; set; }
        public GymExperience? GymExperience { get; set; }
        public GymAccess? GymAccess { get; set; }
        public string[] AvailableEquipment { get; set; } = Array.Empty<string>();
        public bool Injured { get; set; }
        public InjuryArea[] Injuries { get; set; } = [];
        public TrainingFrequency? TrainingFrequency { get; set; }
        public string Sport { get; set; } = string.Empty;
        public DateTime? SeasonStart { get; set; }
        public DateTime? SeasonEnd { get; set; }
        public SportLevel? SportLevel { get; set; }
        public Position? Position { get; set; }
        public Goal[] SportsGoals { get; set; } = [];

    }
}