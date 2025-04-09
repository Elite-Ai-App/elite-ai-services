using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.ComponentModel.DataAnnotations;

namespace EliteAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly Client _supabaseClient;

    public AuthController(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }

    /// <summary>
    /// Login with email and password to get a JWT token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>JWT token if successful</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var session = await _supabaseClient.Auth.SignIn(request.Email, request.Password);
            return Ok(new { Token = session.AccessToken });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Invalid credentials", Error = ex.Message });
        }
    }
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
} 