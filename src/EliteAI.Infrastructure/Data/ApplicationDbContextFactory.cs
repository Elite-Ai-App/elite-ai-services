using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EliteAI.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql("Host=aws-0-ca-central-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.sddwxswbsezboorhimuf;Password=cwc.WHT!bqe*mcr5dxb");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
} 