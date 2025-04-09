using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EliteAI.API.Controllers;

/// <summary>
/// Example secured controller for weather data.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize] // Requires valid JWT token
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpGet("/me")]
    public IActionResult GetUser()
    {

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        return Ok();
    }

    [HttpPost("/upload-profile-picture")]
    public IActionResult UploadProfilePicture(IFormFile file)
    {
        return Ok();
    }

    [HttpPost("/update-username")]
    public IActionResult UpdateUsername(string username)
    {
        return Ok();
    }
}