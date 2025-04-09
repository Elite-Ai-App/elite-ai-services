using EliteAI.Application.DTOs.Profile;
using EliteAI.Application.DTOs.Sports;
using EliteAI.Application.DTOs.User;
using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing users.
/// </summary>
public class UserService
{
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Creates a new instance of UserService.
    /// </summary>
    /// <param name="userRepository">The repository for user operations.</param>
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user or null if not found.</returns>
    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetCompleteProfile(id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        
        var sport = user.Profile?.Sports?.FirstOrDefault();


        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            UnitType = user.UnitType,
            ProfilePictureUrl = user.ProfilePictureUrl,
            OnboardingComplete = user.OnboardingComplete,
            Profile = user.Profile != null ? new ProfileDto
            {
                Id = user.Profile.Id,              
                Height = user.Profile.Height,
                Weight = user.Profile.Weight,
                AgeGroup = user.Profile.AgeGroup,
                Gender = user.Profile.Gender,
                AvailableEquipment = user.Profile.AvailableEquipment,
                GymAccess = user.Profile.GymAccess,
                GymExperience = user.Profile.GymExperience,
                Injured = user.Profile.Injured,
                Injuries = user.Profile.Injuries,
                TrainingFrequency = user.Profile.TrainingFrequency,               
              
            } : null,
            SportsDto = sport != null ? new SportsDto
            {
                Id = sport.Id,               
                Sport = sport.Sport,
                SeasonStart = sport.SeasonStart,
                SeasonEnd = sport.SeasonEnd,
                Position = sport.Position,
                SportLevel = sport.SportLevel,
                Goals = sport.Goals
            } : null
        };

        return userDto;
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="username">The username to search for.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user or null if not found.</returns>
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="data">The user data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user.</returns>
    public async Task<User> CreateUserAsync(User data)
    {
        return await _userRepository.CreateAsync(data);
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="id">The unique identifier of the user to update.</param>
    /// <param name="data">The user data to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user.</returns>
    public async Task<User> UpdateUserAsync(Guid id, User data)
    {
        return await _userRepository.UpdateAsync(data);
    }

    /// <summary>
    /// Saves a username for a user.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="requestedUsername">The requested username.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user.</returns>
    public async Task<User> UpdateUsernameAsync(Guid id, string requestedUsername)
    {
        // Clean the username (remove special characters, standardize format)
        var baseUsername = CleanUsername(requestedUsername);

        // Find the next available number
        var uniqueUsername = await FindNextAvailableUsernameAsync(baseUsername);

        var user = await _userRepository.UpdateAsync(new User
        {
            Id = id,
            UserName = $"{uniqueUsername.baseUsername}#{uniqueUsername.numericSuffix}",
            NumericSuffix = uniqueUsername.numericSuffix,
            BaseUsername = uniqueUsername.baseUsername
        });

        return user;
    }
      

    private string CleanUsername(string username)
    {
        // Remove special characters, replace spaces with underscores, etc.
        return username
            .Trim()
            .ToLower()
            .Replace("[^\\w\\s]", "")
            .Replace("\\s+", "_")
            .Substring(0, Math.Min(20, username.Length)); // Limit length
    }

    private async Task<(string baseUsername, string numericSuffix)> FindNextAvailableUsernameAsync(string baseUsername)
    {
        // Always use a random 4-digit number
        // We'll try 30 random numbers before using a more deterministic approach
        var triedNumbers = new HashSet<int>();

        var candidateUsername = "";

        // First strategy: Try 30 different random 4-digit numbers
        for (var attempt = 0; attempt < 30; attempt++)
        {
            // Generate a random 4-digit number (1000-9999)
            var randomNum = new Random().Next(1000, 10000);

            // Skip if we've already tried this number
            if (triedNumbers.Contains(randomNum))
            {
                continue;
            }

            triedNumbers.Add(randomNum);
            candidateUsername = $"{baseUsername}#{randomNum}";
            var isAvailable = await IsUsernameAvailableAsync(candidateUsername);

            if (isAvailable)
            {
                return (baseUsername, randomNum.ToString());
            }
        }

        // Second strategy: Use a combination of timestamp and random digits
        // This makes collisions extremely unlikely
        var timestampPart = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString().Substring(0, 3);
        var randomPart = new Random().Next(100, 1000).ToString();

        // Combine to make a 4-digit number (3 digits from timestamp + 1 random digit)
        candidateUsername = $"{baseUsername}{timestampPart}{randomPart[0]}";

        // Verify one more time (extremely unlikely to be taken)
        var isAvailable2 = await IsUsernameAvailableAsync(candidateUsername);

        if (isAvailable2)
        {
            return (candidateUsername, randomPart);
        }

        // Last resort: Use full timestamp if all else fails
        // This is virtually guaranteed to be unique
        var fullTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var randomTimeStamp = fullTimestamp.Substring(fullTimestamp.Length - 4).PadLeft(4, '0');
        candidateUsername = $"{baseUsername}#{randomTimeStamp}";
        return (candidateUsername, randomTimeStamp);
    }

    private async Task<bool> IsUsernameAvailableAsync(string username)
    {
        var existingUser = await GetUserByUsernameAsync(username);
        return existingUser == null;
    }
} 