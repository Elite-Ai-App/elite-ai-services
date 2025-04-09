using Microsoft.EntityFrameworkCore;
using EliteAI.Domain.Entities;

namespace EliteAI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<PlayerProfile> PlayerProfiles { get; set; }
    public DbSet<PlayerSports> PlayerSports { get; set; }
    public DbSet<PlayerWorkoutPlan> PlayerWorkoutPlans { get; set; }
    public DbSet<PlayerWorkoutPlanSchedule> PlayerWorkoutPlanSchedules { get; set; }
    public DbSet<PlayerWorkout> PlayerWorkouts { get; set; }
    public DbSet<PlayerWorkoutExercise> PlayerWorkoutExercises { get; set; }
    public DbSet<PlayerWorkoutLog> PlayerWorkoutLogs { get; set; }
    public DbSet<Exercise> Exercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Configure PlayerProfile
        modelBuilder.Entity<PlayerProfile>()
            .HasOne(p => p.User)
            .WithOne(u => u.PlayerProfile)
            .HasForeignKey<PlayerProfile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PlayerSports
        modelBuilder.Entity<PlayerSports>()
            .HasKey(ps => new { ps.PlayerProfileId, ps.Sport });

        // Configure PlayerWorkoutPlan
        modelBuilder.Entity<PlayerWorkoutPlan>()
            .HasOne(p => p.User)
            .WithMany(u => u.PlayerWorkoutPlanSchedules)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PlayerWorkoutPlanSchedule
        modelBuilder.Entity<PlayerWorkoutPlanSchedule>()
            .HasOne(s => s.PlayerWorkoutPlan)
            .WithMany(p => p.Schedules)
            .HasForeignKey(s => s.PlayerWorkoutPlanId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PlayerWorkout
        modelBuilder.Entity<PlayerWorkout>()
            .HasOne(w => w.PlayerWorkoutPlanSchedule)
            .WithOne(s => s.PlayerWorkout)
            .HasForeignKey<PlayerWorkout>(w => w.PlayerWorkoutPlanScheduleId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PlayerWorkoutExercise
        modelBuilder.Entity<PlayerWorkoutExercise>()
            .HasOne(e => e.PlayerWorkout)
            .WithMany(w => w.Exercises)
            .HasForeignKey(e => e.PlayerWorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure PlayerWorkoutLog
        modelBuilder.Entity<PlayerWorkoutLog>()
            .HasOne(l => l.PlayerWorkout)
            .WithOne(w => w.WorkoutLog)
            .HasForeignKey<PlayerWorkoutLog>(l => l.PlayerWorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 