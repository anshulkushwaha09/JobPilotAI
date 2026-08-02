using JobPilot.API.DTOs.Response;
using JobPilot.API.Repositories.Interfaces;
using JobPilot.API.Services.Interfaces;


namespace JobPilot.API.Services.Implementations;


public class ResumeService : IResumeService
{

    private readonly IResumeRepository _repository;

    private readonly IFileService _fileService;


    public ResumeService(
    IResumeRepository repository,
    IFileService fileService)
    {

        _repository = repository;

        _fileService = fileService;

    }



    public async Task<ResumeResponse> UploadAsync(
    int userId,
    IFormFile file,
    bool isDefault)
    {


        var path =
        await _fileService.SaveResumeAsync(
        userId,
        file);



        long id =
        await _repository.UploadResumeAsync(

        userId,

        file.FileName,

        path,

        Path.GetExtension(file.FileName),

        file.Length,

        null,

        isDefault);



        return new ResumeResponse
        {

            ResumeId = id,

            ResumeName = file.FileName,

            ResumeUrl = path,

            FileType = Path.GetExtension(file.FileName),

            FileSize = file.Length,

            IsDefault = isDefault,

            UploadedOn = DateTime.UtcNow

        };


    }




    public Task<List<ResumeResponse>> GetAllAsync(int userId)
    => _repository.GetUserResumesAsync(userId);



    public Task<bool> SetDefaultAsync(long resumeId, int userId)
    => _repository.SetDefaultResumeAsync(resumeId, userId);



    public Task<bool> DeleteAsync(long resumeId, int userId)
    => _repository.DeleteResumeAsync(resumeId, userId);



}