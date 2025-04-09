using EliteAI.Domain.Entities;

namespace EliteAI.Application.Interfaces;

public interface IProfileRepository : IRepository<Profile>
{
    Task<Profile?> GetByUserIdAsync(Guid userId);
    Task<Profile> UpdateInjuriesAsync(Guid id, InjuryArea[] injuries);
    Task<Profile> UpdateEquipmentAsync(Guid id, string[] equipment);
} 