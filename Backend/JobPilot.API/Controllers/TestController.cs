
using JobPilot.API.Services.Email.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/email")]
public class EmailController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("test")]
    public async Task<IActionResult> Test(string email)
    {
        await _emailService.SendAsync(
            email,
            "JobPilot AI Test Email",
            "<h2>Email Service Working Successfully 🚀</h2>");

        return Ok("Email Sent");
    }
}