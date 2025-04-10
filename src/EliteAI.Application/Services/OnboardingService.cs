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
    private readonly IMessagePublisher _messagePublisher;

    /// <summary>
    /// Creates a new instance of OnboardingService.
    /// </summary>
    /// <param name="userRepository">The repository for user operations.</param>
    /// <param name="profileRepository">The repository for profile operations.</param>
    /// <param name="sportsRepository">The repository for sports operations.</param>
    /// <param name="messagePublisher">The message publisher for sending workout generation requests.</param>
    public OnboardingService(
        IUserRepository userRepository,
        IProfileRepository profileRepository,
        ISportsRepository sportsRepository,
        IMessagePublisher messagePublisher)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _sportsRepository = sportsRepository;
        _messagePublisher = messagePublisher;
    }

    /// <summary>
    /// Completes the onboarding process for a user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="data">The onboarding data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user, profile, and sports information.</returns>
    public async Task CompleteOnboardingAsync(Guid userId, CompleteOnboardingDTO data)
    {
        // Update user
        var user = await _userRepository.UpdateAsync(new User
        {
            Id = userId,
            UnitType = data.UnitType,
            OnboardingComplete = true
        });

        // 2. Create or update Profile
        var existingProfile = await _profileRepository.GetByUserIdAsync(userId);

        Profile profile;
        if (existingProfile != null)
        {
            existingProfile.Height = data.Height;
            existingProfile.Weight = data.Weight;
            existingProfile.Gender = data.Gender;
            existingProfile.AgeGroup = data.AgeGroup;
            existingProfile.GymExperience = data.GymExperience;
            existingProfile.GymAccess = data.GymAccess;
            existingProfile.AvailableEquipment = data.AvailableEquipment;
            existingProfile.UnitType = data.UnitType;
            existingProfile.Injured = data.Injured;
            existingProfile.Injuries = data.Injuries;
            existingProfile.TrainingFrequency = data.TrainingFrequency;

            profile = await _profileRepository.UpdateAsync(existingProfile);
        }
        else
        {
            profile = await _profileRepository.CreateAsync(new Profile
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
        }

        // 3. Create or update Sports
        var existingSports = await _sportsRepository.GetByProfileIDAndSportAsync(profile.Id, Sport.BASKETBALL);

        if (existingSports != null)
        {
            existingSports.Sport = Sport.BASKETBALL;
            existingSports.SeasonStart = data.SeasonStart;
            existingSports.SeasonEnd = data.SeasonEnd;
            existingSports.SportLevel = data.SportLevel;
            existingSports.Position = data.Position;
            existingSports.Goals = data.SportsGoals;

            await _sportsRepository.UpdateAsync(existingSports);
        }
        else
        {
            await _sportsRepository.CreateAsync(new Sports
            {
                Profile = profile,
                Sport = Sport.BASKETBALL,
                SeasonStart = data.SeasonStart,
                SeasonEnd = data.SeasonEnd,
                SportLevel = data.SportLevel,
                Position = data.Position,
                Goals = data.SportsGoals
            });
        }

        // Send message to RabbitMQ to generate workout plan
        await _messagePublisher.PublishWorkoutGenerationRequest(userId.ToString());
       
    }
}