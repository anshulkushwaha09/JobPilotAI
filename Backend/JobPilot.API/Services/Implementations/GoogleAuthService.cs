using Google.Apis.Auth;
using JobPilot.API.Configurations;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace JobPilot.API.Services.Implementations;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly GoogleAuthSettings _settings;

    public GoogleAuthService(IOptions<GoogleAuthSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<GoogleUserResponse?> VerifyGoogleTokenAsync(string idToken)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                idToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _settings.ClientId }
                });

            return new GoogleUserResponse
            {
                GoogleId = payload.Subject,
                Email = payload.Email,
                FullName = payload.Name,
                Picture = payload.Picture
            };
        }
        catch
        {
            return null;
        }
    }
}