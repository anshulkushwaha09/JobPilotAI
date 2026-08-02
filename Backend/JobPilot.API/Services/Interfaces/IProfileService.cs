using JobPilot.API.DTOs.Profile;

namespace JobPilot.API.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileResponse?> GetProfileAsync(int userId);

    Task<bool> UpdateProfileAsync(
        int userId,
        UpdateProfileRequest request);
}