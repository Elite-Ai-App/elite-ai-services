using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.ComponentModel.DataAnnotations;

namespace EliteAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
 

    public AuthController()
    {
   
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
            var supabaseUrl = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var supabaseAnonKey = Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY");
           

           var supabaseClient = new Supabase.Client(supabaseUrl, supabaseAnonKey);

            var session = await supabaseClient.Auth.SignIn(request.Email, request.Password);
            return Ok(new { Token = session?.AccessToken });
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