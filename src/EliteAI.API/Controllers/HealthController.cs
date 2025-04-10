using Microsoft.AspNetCore.Mvc;
using Supabase;
using System.Net.Http;
using System.Threading.Tasks;

namespace EliteAI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly Client _supabaseClient;
    private readonly IHttpClientFactory _httpClientFactory;

    public HealthController(Client supabaseClient, IHttpClientFactory httpClientFactory)
    {
        _supabaseClient = supabaseClient;
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Basic health check endpoint
    /// </summary>
    /// <returns>Status of the API</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }

}