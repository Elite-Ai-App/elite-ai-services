using System.Security.Claims;
using System.Threading.Tasks;
using EliteAI.Application.Api;
using EliteAI.Application.DTOs.Onboarding;
using EliteAI.Application.Services;
using EliteAI.Domain.Entities;
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
    private readonly OnboardingService _onboardingService;

    public OnBoardingController(ILogger<OnBoardingController> logger, OnboardingService onboardingService)
    {
        _logger = logger;
        _onboardingService = onboardingService;
    }

    [HttpPost("complete")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CompleteOnboarding([FromBody] CompleteOnboardingDTO data)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

         await _onboardingService.CompleteOnboardingAsync(userId, data);

        return Ok(new ApiResponse<object>{ Success = true, Message = "Onboarding Complete, Workout Generationg Inprogress" }); ;
    }
}