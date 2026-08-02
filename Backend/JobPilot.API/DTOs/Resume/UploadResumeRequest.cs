using Microsoft.AspNetCore.Http;

namespace JobPilot.API.DTOs.Resume;

public class UploadResumeRequest
{
    public IFormFile Resume { get; set; } = default!;

    public bool IsDefault { get; set; } = true;
}