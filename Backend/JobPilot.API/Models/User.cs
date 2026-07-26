namespace JobPilot.API.Models;

public class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsGoogleUser { get; set; }
    public int RoleId { get; set; }
    public string GoogleId { get; set; }
    public string AuthProvider { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }
}