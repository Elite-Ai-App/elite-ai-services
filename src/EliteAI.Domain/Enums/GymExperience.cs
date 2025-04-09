namespace EliteAI.Domain.Enums;

/// <summary>
/// Represents the gym experience level of a user.
/// </summary>
public enum GymExperience
{
    /// <summary>
    /// No prior gym experience
    /// </summary>
    BEGINNER,

    /// <summary>
    /// Some gym experience (1-6 months)
    /// </summary>
    INTERMEDIATE,

    /// <summary>
    /// Significant gym experience (6+ months)
    /// </summary>
    ADVANCED,

    /// <summary>
    /// Professional or competitive level experience
    /// </summary>
    Professional
} 