using EliteAI.Domain.Entities;

namespace EliteAI.Application.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetCompleteProfile(Guid id);
} 