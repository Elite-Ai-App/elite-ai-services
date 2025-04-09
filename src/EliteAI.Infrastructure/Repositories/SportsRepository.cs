using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class SportsRepository : Repository<Sports>, ISportsRepository
{
    public SportsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Sports>> GetByProfileIdAsync(Guid profileId)
    {
        return await _dbSet.Where(s => s.ProfileId == profileId).ToListAsync();
    }

    public async Task<Sports> UpdateSportLevelAsync(Guid id, SportLevel sportLevel)
    {
        var sports = await GetByIdAsync(id);
        if (sports == null)
            throw new KeyNotFoundException($"Sports with ID {id} not found.");

        sports.SportLevel = sportLevel;
        return await UpdateAsync(sports);
    }

    public async Task<Sports> UpdateSeasonDatesAsync(Guid id, DateTime? seasonStart, DateTime? seasonEnd)
    {
        var sports = await GetByIdAsync(id);
        if (sports == null)
            throw new KeyNotFoundException($"Sports with ID {id} not found.");

        sports.SeasonStart = seasonStart;
        sports.SeasonEnd = seasonEnd;
        return await UpdateAsync(sports);
    }

    public async Task<Sports> UpdateGoalsAsync(Guid id, Goal[] goals)
    {
        var sports = await GetByIdAsync(id);
        if (sports == null)
            throw new KeyNotFoundException($"Sports with ID {id} not found.");

        sports.Goals = goals;
        return await UpdateAsync(sports);
    }
} 