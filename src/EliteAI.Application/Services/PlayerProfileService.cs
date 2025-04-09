using EliteAI.Application.Interfaces;
using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing player profiles.
/// </summary>
public class ProfileService
{
    private readonly IProfileRepository _profileRepository;

    /// <summary>
    /// Creates a new instance of ProfileService.
    /// </summary>
    /// <param name="profileRepository">The repository for player profile operations.</param>
    public ProfileService(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    /// <summary>
    /// Retrieves a player profile by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the profile.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the profile or null if not found.</returns>
    public async Task<Profile?> GetProfileByIdAsync(Guid id)
    {
        return await _profileRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves a player profile by user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the profile or null if not found.</returns>
    public async Task<Profile?> GetProfileByUserIdAsync(Guid userId)
    {
        return await _profileRepository.GetByUserIdAsync(userId);
    }

    /// <summary>
    /// Creates a new player profile.
    /// </summary>
    /// <param name="data">The profile data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created profile.</returns>
    public async Task<Profile> CreateProfileAsync(Profile data)
    {
        return await _profileRepository.CreateAsync(data);
    }

    /// <summary>
    /// Updates an existing player profile.
    /// </summary>
    /// <param name="id">The unique identifier of the profile to update.</param>
    /// <param name="data">The profile data to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated profile.</returns>
    public async Task<Profile> UpdateProfileAsync(Guid id, Profile data)
    {
        return await _profileRepository.UpdateAsync(data);
    }

    
    /// <summary>
    /// Updates the injuries for a player profile.
    /// </summary>
    /// <param name="id">The unique identifier of the profile.</param>
    /// <param name="injuries">Array of injury areas to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated profile.</returns>
    public async Task<Profile> UpdateInjuriesAsync(Guid id, InjuryArea[] injuries)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        if (profile == null)
        {
            throw new KeyNotFoundException($"Profile with ID {id} not found.");
        }

        profile.Injuries = injuries;
        return await _profileRepository.UpdateAsync(profile);
    }

    /// <summary>
    /// Updates the available equipment for a player profile.
    /// </summary>
    /// <param name="id">The unique identifier of the profile.</param>
    /// <param name="equipment">Array of equipment strings to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated profile.</returns>
    public async Task<Profile> UpdateEquipmentAsync(Guid id, string[] equipment)
    {
        var profile = await _profileRepository.GetByIdAsync(id);
        if (profile == null)
        {
            throw new KeyNotFoundException($"Profile with ID {id} not found.");
        }

        profile.AvailableEquipment = equipment;
        return await _profileRepository.UpdateAsync(profile);
    }
} 