using Microsoft.EntityFrameworkCore;
using EliteAI.Domain.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace EliteAI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Profile> PlayerProfiles { get; set; }

    public DbSet<Sports> PlayerSports { get; set; }

    public DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    public DbSet<WorkoutPlanSchedule> WorkoutSchedules { get; set; }

    public DbSet<Workout> Workouts { get; set; }

    public DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    public DbSet<Exercise> Exercises { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");

        // Configure User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Id)
            .IsUnique();

        // Configure Profile
        modelBuilder.Entity<Profile>()
            .HasOne(p => p.User)
            .WithOne(u => u.Profile)
            .HasForeignKey<Profile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure Sports
        modelBuilder.Entity<Sports>()
            .HasOne(s => s.Profile)
            .WithMany(p => p.Sports)
            .HasForeignKey(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

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

        // Configure Exercise
        modelBuilder.Entity<Exercise>()
            .HasMany(e => e.WorkoutExercises)
            .WithOne(we => we.Exercise)
            .HasForeignKey(we => we.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure WorkoutLog
        modelBuilder.Entity<WorkoutLog>()
            .HasOne(l => l.Workout)
            .WithOne(w => w.WorkoutLog)
            .HasForeignKey<WorkoutLog>(l => l.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}