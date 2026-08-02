using JobPilot.API.DTOs.Response;
using Microsoft.AspNetCore.Http;

namespace JobPilot.API.Services.Interfaces;


public interface IResumeService
{

    Task<ResumeResponse> UploadAsync(
    int userId,
    IFormFile file,
    bool isDefault);


    Task<List<ResumeResponse>> GetAllAsync(
    int userId);


    Task<bool> SetDefaultAsync(
    long resumeId,
    int userId);


    Task<bool> DeleteAsync(
    long resumeId,
    int userId);


}