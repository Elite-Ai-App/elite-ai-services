using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApi.API.Controllers;

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
    public IActionResult GetOnBoarding()
    {
        // Simulated data - normally call a service layer here
        var weatherData = new
        {
            Date = DateTime.UtcNow,
            TemperatureC = 18,
            Summary = "Cloudy"
        };

        _logger.LogInformation("Weather retrieved for {Date}", weatherData.Date);

        return Ok(weatherData);
    }
}