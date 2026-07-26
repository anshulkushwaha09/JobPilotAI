using JobPilot.API.DTOs.Response;

namespace JobPilot.API.Services.Interfaces;

public interface IGoogleAuthService
{
    Task<GoogleUserResponse?> VerifyGoogleTokenAsync(string idToken);
}