namespace JobPilot.API.Models.Entities;

public class PasswordResetToken
{
    public long ResetId { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedOn { get; set; }
}