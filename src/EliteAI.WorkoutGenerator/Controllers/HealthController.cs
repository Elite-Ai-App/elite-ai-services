using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EliteAI.WorkoutGenerator.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly HealthCheckService _healthCheckService;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        HealthCheckService healthCheckService,
        ILogger<HealthController> logger)
    {
        _healthCheckService = healthCheckService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var report = await _healthCheckService.CheckHealthAsync();
        var status = report.Status == HealthStatus.Healthy ? "Healthy" : "Unhealthy";

        _logger.LogInformation("Health check status: {Status}", status);

        return new JsonResult(new
        {
            status = status,
            timestamp = DateTime.UtcNow,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.TotalMilliseconds
            })
        });
    }

    [HttpGet("live")]
    public IActionResult Liveness()
    {
        return Ok(new
        {
            status = "Alive",
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("ready")]
    public async Task<IActionResult> Readiness()
    {
        var report = await _healthCheckService.CheckHealthAsync();
        return report.Status == HealthStatus.Healthy
            ? Ok(new { status = "Ready", timestamp = DateTime.UtcNow })
            : StatusCode(503, new { status = "Not Ready", timestamp = DateTime.UtcNow });
    }
} 