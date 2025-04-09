using EliteAI.Domain;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;

namespace EliteAI.Application.Interfaces;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile?> GetByUserIdAsync(Guid userId);
    Task<Profile> UpdateInjuriesAsync(Guid id, InjuryArea[] injuries);
    Task<Profile> UpdateEquipmentAsync(Guid id, string[] equipment);
} 