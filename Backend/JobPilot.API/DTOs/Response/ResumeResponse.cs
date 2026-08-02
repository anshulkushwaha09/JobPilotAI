namespace JobPilot.API.DTOs.Response;

public class ResumeResponse
{
    public long ResumeId { get; set; }

    public string ResumeName { get; set; } = string.Empty;

    public string ResumeUrl { get; set; } = string.Empty;

    public string FileType { get; set; } = string.Empty;

    public long FileSize { get; set; }

    public int VersionNo { get; set; }

    public bool IsDefault { get; set; }

    public DateTime UploadedOn { get; set; }
}