namespace JobPilot.API.DTOs.Request;

public class LogoutRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}