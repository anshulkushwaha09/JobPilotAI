using Microsoft.AspNetCore.Http;

namespace JobPilot.API.DTOs.Request;

public class UploadResumeRequest
{
    public IFormFile File { get; set; } = null!;

    public bool IsDefault { get; set; }
}