using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Interfaces;

public interface ISportsRepository : IRepository<Sports>
{
    Task<IEnumerable<Sports>> GetByProfileIdAsync(Guid profileId);
    Task<Sports> UpdateSportLevelAsync(Guid id, SportLevel sportLevel);
    Task<Sports> UpdateSeasonDatesAsync(Guid id, DateTime? seasonStart, DateTime? seasonEnd);
    Task<Sports> UpdateGoalsAsync(Guid id, Goal[] goals);
} 