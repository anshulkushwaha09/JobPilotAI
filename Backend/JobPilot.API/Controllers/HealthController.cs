using Microsoft.AspNetCore.Mvc;

namespace JobPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Status = "Healthy",
            Application = "JobPilot API",
            Version = "1.0.0",
            ServerTime = DateTime.UtcNow
        });
    }
}