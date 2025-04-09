using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EliteAI.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=db.sddwxswbsezboorhimuf.supabase.co;Database=postgres;Username=postgres;Password=sUNxVXgsQKuM2L7B;SSL Mode=Require;Trust Server Certificate=true");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}