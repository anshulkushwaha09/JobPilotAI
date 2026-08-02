using JobPilot.API.DTOs.Profile;
using JobPilot.API.Repositories.Interfaces;
using JobPilot.API.Services.Interfaces;

namespace JobPilot.API.Services.Implementations;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _repository;

    public ProfileService(IProfileRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProfileResponse?> GetProfileAsync(
        int userId)
    {
        return await _repository.GetProfileAsync(userId);
    }

    public async Task<bool> UpdateProfileAsync(
        int userId,
        UpdateProfileRequest request)
    {
        return await _repository.UpdateProfileAsync(
            userId,
            request);
    }
}