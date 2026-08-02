using JobPilot.API.DTOs.Profile;

namespace JobPilot.API.Repositories.Interfaces;

public interface IProfileRepository
{
    Task<ProfileResponse?> GetProfileAsync(int userId);

    Task<bool> UpdateProfileAsync(
        int userId,
        UpdateProfileRequest request);
}