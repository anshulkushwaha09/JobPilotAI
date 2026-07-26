namespace JobPilot.API.Models.Entities;

public class RefreshToken
{
    public long TokenId { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedOn { get; set; }
}