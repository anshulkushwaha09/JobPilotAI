using JobPilot.API.DTOs.Response;

namespace JobPilot.API.Repositories.Interfaces;

public interface IResumeRepository
{

    Task<long> UploadResumeAsync(
        int userId,
        string resumeName,
        string resumeUrl,
        string fileType,
        long fileSize,
        string? resumeText,
        bool isDefault);


    Task<List<ResumeResponse>> GetUserResumesAsync(
        int userId);


    Task<ResumeResponse?> GetDefaultResumeAsync(
        int userId);


    Task<bool> SetDefaultResumeAsync(
        long resumeId,
        int userId);


    Task<bool> DeleteResumeAsync(
        long resumeId,
        int userId);


    Task<ResumeResponse?> GetResumeByIdAsync(
        long resumeId,
        int userId);

}