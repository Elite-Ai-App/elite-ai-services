using EliteAI.Application.DTOs.Onboarding;
using EliteAI.Application.Interfaces;
using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing user onboarding.
/// </summary>
public class OnboardingService
{
    private readonly IUserRepository _userRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ISportsRepository _sportsRepository;

    /// <summary>
    /// Creates a new instance of OnboardingService.
    /// </summary>
    /// <param name="userRepository">The repository for user operations.</param>
    /// <param name="profileRepository">The repository for profile operations.</param>
    /// <param name="sportsRepository">The repository for sports operations.</param>
    public OnboardingService(
        IUserRepository userRepository,
        IProfileRepository profileRepository,
        ISportsRepository sportsRepository)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _sportsRepository = sportsRepository;
    }

    /// <summary>
    /// Completes the onboarding process for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="data">The onboarding data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user, profile, and sports information.</returns>
    public async Task<(User User, Profile Profile, Sports Sports)> CompleteOnboardingAsync(Guid userId, CompleteOnboardingDTO data)
    {
        // Update user
        var user = await _userRepository.UpdateAsync(new User
        {
            Id = userId,
            UnitType = data.UnitType,
            OnboardingComplete = true
        });

        // Create or update player profile
        var profile = await _profileRepository.CreateAsync(new Profile
        {
            UserId = userId,
            Height = data.Height,
            Weight = data.Weight,
            Gender = data.Gender,
            AgeGroup = data.AgeGroup,
            GymExperience = data.GymExperience,
            GymAccess = data.GymAccess,
            AvailableEquipment = data.AvailableEquipment,
            UnitType = data.UnitType,
            Injured = data.Injured,
            Injuries = data.Injuries,
            TrainingFrequency = data.TrainingFrequency
        });

        // Create player sports record
        var sports = await _sportsRepository.CreateAsync(new Sports
        {
            ProfileId = profile.Id,
            Sport = Sport.BASKETBALL,
            SeasonStart = data.SeasonStart,
            SeasonEnd = data.SeasonEnd,
            SportLevel = data.SportLevel,
            Position = data.Position,
            Goals = data.SportsGoals
        });

        // TODO: Send message to RabbitMQ to generate workout plan
        // This will be implemented when we set up the message queue

        return (user, profile, sports);
    }
}