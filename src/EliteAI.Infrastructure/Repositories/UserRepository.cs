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

    public async Task<User?> GetCompleteProfile(Guid id)
    {

        return await _dbSet
                    .Include(u => u.Profile)
                    .ThenInclude(u => u.Sports)
                    .FirstOrDefaultAsync(u => u.Id == id);
    }
} 