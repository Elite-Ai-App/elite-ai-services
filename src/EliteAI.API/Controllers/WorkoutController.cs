using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteAI.API.Controllers;

/// <summary>
/// Example secured controller for weather data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires valid JWT token
public class WorkoutController : ControllerBase
{
    private readonly ILogger<WorkoutController> _logger;

    public WorkoutController(ILogger<WorkoutController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/{id}")]
    public IActionResult GetWorkout(string id)
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpGet("/plan")]
    public IActionResult GetActivePlan()
    {
        return Ok();
    }

    [HttpGet("/plan/{startDate}/{endDate}")]
    public IActionResult GetWorkoutsByDateRange(DateTime startDate, DateTime endDate)
    {
        return Ok();
    }
    
    
}