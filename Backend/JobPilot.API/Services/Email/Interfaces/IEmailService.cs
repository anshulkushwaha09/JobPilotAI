namespace JobPilot.API.Services.Email.Interfaces;

public interface IEmailService
{
    Task SendAsync(
        string to,
        string subject,
        string htmlBody);
}