using System.ComponentModel.DataAnnotations;

namespace EliteAI.Application.DTOs.Base;

public abstract class BaseDto
{
    public Guid Id { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; }
} 