using JobPilot.API.DTOs.Request;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Models.Request;

namespace JobPilot.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);

    Task<AuthResponse?> LoginAsync(LoginRequest request);

    Task<AuthResponse?> GoogleLoginAsync(GoogleLoginRequest request);
    Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    Task<bool> LogoutAsync(LogoutRequest request);

    Task<bool> ChangePasswordAsync(
  int userId,
  ChangePasswordRequest request);

    Task<bool> ForgotPasswordAsync(
    ForgotPasswordRequest request);

    Task<bool> ResetPasswordAsync(
        ResetPasswordRequest request);

}