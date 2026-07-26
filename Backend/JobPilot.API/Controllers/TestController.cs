using Microsoft.AspNetCore.Mvc;

namespace JobPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Success = true,
            Message = "JobPilot API is running successfully."
        });
    }
}