using System.ComponentModel.DataAnnotations;
using EliteAI.Application.DTOs.Base;
using EliteAI.Domain;

namespace EliteAI.Application.DTOs.User;

public class UserDto : BaseDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public UnitType UnitType { get; set; }

    [Url]
    public string? ProfilePictureUrl { get; set; }

    public bool OnboardingComplete { get; set; }
}

public class CreateUserDto
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public UnitType UnitType { get; set; }
}

public class UpdateUserDto
{
    [StringLength(50)]
    public string? FirstName { get; set; }

    [StringLength(50)]
    public string? LastName { get; set; }

    [StringLength(50)]
    public string? UserName { get; set; }

    public UnitType? UnitType { get; set; }

    [Url]
    public string? ProfilePictureUrl { get; set; }
} 