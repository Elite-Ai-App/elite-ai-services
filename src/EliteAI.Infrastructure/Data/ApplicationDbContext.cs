using Microsoft.EntityFrameworkCore;
using EliteAI.Domain.Entities;

namespace EliteAI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Profile> PlayerProfiles { get; set; }
    public DbSet<Sports> PlayerSports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");

        // Configure User
        modelBuilder.Entity<User>()           
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Configure Profile
        modelBuilder.Entity<Profile>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<Profile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Sports
        modelBuilder.Entity<Sports>()
            .HasKey(ps => new { ps.ProfileId, ps.Sport });

        // Configure WorkoutPlan
        modelBuilder.Entity<WorkoutPlan>()
            .HasOne(p => p.User)
            .WithMany(u => u.WorkoutPlanSchedules)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure WorkoutPlanSchedule
        modelBuilder.Entity<WorkoutPlanSchedule>()
            .HasOne(s => s.WorkoutPlan)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.WorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Workout
        modelBuilder.Entity<Workout>()
            .HasOne(w => w.WorkoutPlanSchedule)
            .WithOne(s => s.Workout)
            .HasForeignKey<Workout>(w => w.WorkoutPlanScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure WorkoutExercise
        modelBuilder.Entity<WorkoutExercise>()
            .HasOne(e => e.Workout)
            .WithMany(w => w.Exercises)
            .HasForeignKey(e => e.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure WorkoutLog
        modelBuilder.Entity<WorkoutLog>()
            .HasOne(l => l.Workout)
            .WithOne(w => w.WorkoutLog)
            .HasForeignKey<WorkoutLog>(l => l.WorkoutId)            
            .OnDelete(DeleteBehavior.Cascade);
    }
} 