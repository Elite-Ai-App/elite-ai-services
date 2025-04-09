using EliteAI.Application.Interfaces;
using EliteAI.Domain.Entities;
using EliteAI.Domain.Enums;
using EliteAI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EliteAI.Infrastructure.Repositories;

public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
{
    public ExerciseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Exercise>> GetByTypeAsync(ExerciseType type)
    {
        return await _dbSet.Where(e => e.Type == type).ToListAsync();
    }
} 