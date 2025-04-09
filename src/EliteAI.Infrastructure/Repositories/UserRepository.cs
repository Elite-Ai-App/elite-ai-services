using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
} 