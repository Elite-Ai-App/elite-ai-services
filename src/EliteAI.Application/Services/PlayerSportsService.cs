using EliteAI.Application.Interfaces;
using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Services;

/// <summary>
/// Service class for managing player sports information.
/// </summary>
public class SportsService
{
    private readonly ISportsRepository _sportsRepository;

    /// <summary>
    /// Creates a new instance of SportsService.
    /// </summary>
    /// <param name="sportsRepository">The repository for player sports operations.</param>
    public SportsService(ISportsRepository sportsRepository)
    {
        _sportsRepository = sportsRepository;
    }

    /// <summary>
    /// Retrieves sports information by its ID.
    /// </summary>
    /// <param name="id">The unique identifier of the sports information.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the sports information or null if not found.</returns>
    public async Task<Sports?> GetSportsByIdAsync(Guid id)
    {
        return await _sportsRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Retrieves all sports information for a player profile.
    /// </summary>
    /// <param name="ProfileId">The unique identifier of the player profile.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of sports information.</returns>
    public async Task<IEnumerable<Sports>> GetSportsByProfileIdAsync(Guid ProfileId)
    {
        return await _sportsRepository.GetByProfileIdAsync(ProfileId);
    }

    /// <summary>
    /// Creates new sports information.
    /// </summary>
    /// <param name="data">The sports data.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created sports information.</returns>
    public async Task<Sports> CreatePlayerSportAsync(Sports data)
    {
        return await _sportsRepository.CreateAsync(data);
    }

    /// <summary>
    /// Updates existing sports information.
    /// </summary>
    /// <param name="id">The unique identifier of the sports information to update.</param>
    /// <param name="data">The sports data to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated sports information.</returns>
    public async Task<Sports> UpdateSportsAsync(Guid id, Sports data)
    {      

       
        return await _sportsRepository.UpdateAsync(data);
    }

  

    /// <summary>
    /// Updates the sport level for a player.
    /// </summary>
    /// <param name="id">The unique identifier of the sports information.</param>
    /// <param name="sportLevel">The new sport level to set.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated sports information.</returns>
    public async Task<Sports> UpdateSportLevelAsync(Guid id, SportLevel sportLevel)
    {
        var sports = await _sportsRepository.GetByIdAsync(id);
        if (sports == null)
        {
            throw new KeyNotFoundException($"Sports information with ID {id} not found.");
        }

        sports.SportLevel = sportLevel;
        return await _sportsRepository.UpdateAsync(sports);
    }

    /// <summary>
    /// Updates the season dates for a player's sport.
    /// </summary>
    /// <param name="id">The unique identifier of the sports information.</param>
    /// <param name="seasonStart">The start date of the season.</param>
    /// <param name="seasonEnd">The end date of the season.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated sports information.</returns>
    public async Task<Sports> UpdateSeasonDatesAsync(Guid id, DateTime? seasonStart, DateTime? seasonEnd)
    {
        var sports = await _sportsRepository.GetByIdAsync(id);
        if (sports == null)
        {
            throw new KeyNotFoundException($"Sports information with ID {id} not found.");
        }

        sports.SeasonStart = seasonStart;
        sports.SeasonEnd = seasonEnd;
        return await _sportsRepository.UpdateAsync(sports);
    }

    /// <summary>
    /// Updates the sports goals for a player.
    /// </summary>
    /// <param name="id">The unique identifier of the sports information.</param>
    /// <param name="sportsGoals">Array of goals to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated sports information.</returns>
    public async Task<Sports> UpdateSportsGoalsAsync(Guid id, Goal[] sportsGoals)
    {
        var sports = await _sportsRepository.GetByIdAsync(id);
        if (sports == null)
        {
            throw new KeyNotFoundException($"Sports information with ID {id} not found.");
        }

        sports.Goals = sportsGoals;
        return await _sportsRepository.UpdateAsync(sports);
    }
} 