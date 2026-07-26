using JobPilot.API.DTOs.Response;
using JobPilot.API.Models;
using JobPilot.API.Models.Entities;

namespace JobPilot.API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<AuthResponse?> RegisterAsync(
        string fullName,
        string email,
        string passwordHash,
        int roleId);

    Task<User?> GetUserByEmailAsync(string email);

    Task UpdateLastLoginAsync(int userId);

    Task<User?> GetUserByGoogleIdAsync(string googleId);

    Task<AuthResponse?> RegisterGoogleUserAsync(
        GoogleUserResponse googleUser);

    Task LinkGoogleAccountAsync(
        int userId,
        string googleId,
        string pictureUrl);

    Task SaveRefreshTokenAsync(
    int userId,
    string token,
    DateTime expiry);

    Task<RefreshToken?> GetRefreshTokenAsync(
        string token);

    Task RevokeRefreshTokenAsync(
        string token);

    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> IsRefreshTokenValidAsync(string refreshToken);
    Task<bool> ChangePasswordAsync(
    int userId,
    string passwordHash);


    Task SavePasswordResetTokenAsync(
    int userId,
    string token,
    DateTime expiry);

    Task<PasswordResetToken?> GetPasswordResetTokenAsync(
        string token);

    Task MarkPasswordResetTokenUsedAsync(
        string token);

    Task UpdatePasswordAsync(
        int userId,
        string passwordHash);


}