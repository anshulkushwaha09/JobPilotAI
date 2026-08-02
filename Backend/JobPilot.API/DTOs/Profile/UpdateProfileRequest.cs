namespace JobPilot.API.DTOs.Profile;

public class UpdateProfileRequest
{
    public string FullName { get; set; } = string.Empty;

    public string? PhoneNumber { get; set; }

    public decimal? Experience { get; set; }

    public string? CurrentCompany { get; set; }

    public string? CurrentDesignation { get; set; }

    public decimal? CurrentCTC { get; set; }

    public decimal? ExpectedCTC { get; set; }

    public int? NoticePeriod { get; set; }

    public string? LinkedInUrl { get; set; }

    public string? GitHubUrl { get; set; }

    public string? PortfolioUrl { get; set; }
}