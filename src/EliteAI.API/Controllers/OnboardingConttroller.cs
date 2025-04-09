using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteAI.API.Controllers;

/// <summary>
/// Example secured controller for weather data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires valid JWT token
public class OnBoardingController : ControllerBase
{
    private readonly ILogger<OnBoardingController> _logger;

    public OnBoardingController(ILogger<OnBoardingController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult CompleteOnBoarding(CompleteOnBoardingDto completeOnBoardingDto)
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        return Ok();
    }
}