using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EliteAI.Domain.Enums;

namespace EliteAI.Domain.Entities;

[Table("player_workout_plan_schedules")]
public class WorkoutPlanSchedule
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("player_workout_plan_id")]
    public Guid WorkoutPlanId { get; set; }

    [ForeignKey("WorkoutPlanId")]
    public virtual WorkoutPlan WorkoutPlan { get; set; } = null!;

    [Column("day_of_week")]
    public TrainingDayOfWeek DayOfWeek { get; set; }

    [Column("workout_plan_name")]
    public string WorkoutPlanName { get; set; } = null!;

    [Column("is_rest_day")]
    public bool IsRestDay { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    public virtual Workout? Workout { get; set; }
} 