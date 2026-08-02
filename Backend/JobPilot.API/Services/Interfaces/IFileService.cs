namespace JobPilot.API.Services.Interfaces;


public interface IFileService
{

    Task<string> SaveResumeAsync(
    int userId,
    IFormFile file);


    bool DeleteFile(string path);


}