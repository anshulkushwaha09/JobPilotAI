using JobPilot.API.Services.Interfaces;
using Microsoft.Extensions.Options;
using JobPilot.API.Configurations;


namespace JobPilot.API.Services.Implementations;


public class FileService : IFileService
{

    private readonly FileStorageSettings _settings;


    public FileService(
    IOptions<FileStorageSettings> settings)
    {
        _settings = settings.Value;
    }



    public async Task<string> SaveResumeAsync(
    int userId,
    IFormFile file)
    {


        string folder =
        Path.Combine(
        Directory.GetCurrentDirectory(),
        _settings.ResumePath,
        userId.ToString(),
        "Resume"
        );



        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);



        string extension =
        Path.GetExtension(file.FileName);



        string fileName =
        $"{Guid.NewGuid()}{extension}";



        string fullPath =
        Path.Combine(folder, fileName);



        using FileStream stream =
        new(fullPath, FileMode.Create);


        await file.CopyToAsync(stream);



        return Path.Combine(
        _settings.ResumePath,
        userId.ToString(),
        "Resume",
        fileName);

    }



    public bool DeleteFile(string path)
    {

        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }

        return false;

    }


}