namespace JobPilot.API.DTOs.Response;

public class GoogleUserResponse
{
    public string GoogleId { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Picture { get; set; } = string.Empty;

    public bool EmailVerified { get; set; }
}