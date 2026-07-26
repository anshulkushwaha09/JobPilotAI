using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        return Ok(new
        {
            Message = "JWT Working Successfully",

            UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,

            Name = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value,

            Email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,

            Role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}