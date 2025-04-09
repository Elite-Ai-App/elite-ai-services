using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class ProfileRepository : Repository<Profile>, IProfileRepository
{
    public ProfileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Profile?> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<Profile> UpdateInjuriesAsync(Guid id, InjuryArea[] injuries)
    {
        var profile = await GetByIdAsync(id);
        if (profile == null)
            throw new KeyNotFoundException($"Profile with ID {id} not found.");

        profile.Injuries = injuries;
        return await UpdateAsync(profile);
    }

    public async Task<Profile> UpdateEquipmentAsync(Guid id, string[] equipment)
    {
        var profile = await GetByIdAsync(id);
        if (profile == null)
            throw new KeyNotFoundException($"Profile with ID {id} not found.");

        profile.AvailableEquipment = equipment;
        return await UpdateAsync(profile);
    }
} 