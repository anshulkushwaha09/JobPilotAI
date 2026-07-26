using JobPilot.API.Constants;
using JobPilot.API.DTOs.Request;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Helpers;
using JobPilot.API.Models.Request;
using JobPilot.API.Repositories.Interfaces;
using JobPilot.API.Services.Email.Interfaces;
using JobPilot.API.Services.Interfaces;

namespace JobPilot.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly JwtTokenHelper _jwtHelper;
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IEmailService _emailService;

    private readonly IConfiguration _configuration;

    public AuthService(
        IAuthRepository repository,
        IGoogleAuthService googleAuthService,
        JwtTokenHelper jwtHelper,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _repository = repository;
        _googleAuthService = googleAuthService;
        _jwtHelper = jwtHelper;
        _emailService = emailService;
        _configuration = configuration;
    }

    #region Register

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        var existingUser =
            await _repository.GetUserByEmailAsync(request.Email);

        if (existingUser != null)
            return null;

        string passwordHash =
            PasswordHelper.HashPassword(request.Password);

        var response =
            await _repository.RegisterAsync(
                request.FullName,
                request.Email,
                passwordHash,
                Roles.JobSeeker);

        if (response == null)
            return null;

        var user =
            await _repository.GetUserByEmailAsync(request.Email);

        if (user == null)
            return null;

        await _repository.UpdateLastLoginAsync(user.UserId);

        var jwt = _jwtHelper.GenerateToken(user);

        string refreshToken =
            _jwtHelper.GenerateRefreshToken();

        DateTime refreshExpiry =
            DateTime.UtcNow.AddDays(30);

        await _repository.SaveRefreshTokenAsync(
            user.UserId,
            refreshToken,
            refreshExpiry);

        response.AccessToken = jwt.Token;
        response.RefreshToken = refreshToken;
        response.Expiry = jwt.Expiry;
        response.RoleId = user.RoleId;
        response.IsNewUser = true;

        return response;
    }

    #endregion

    #region Login

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user =
            await _repository.GetUserByEmailAsync(request.Email);

        if (user == null)
            return null;

        if (!PasswordHelper.VerifyPassword(
            request.Password,
            user.PasswordHash))
            return null;

        await _repository.UpdateLastLoginAsync(user.UserId);

        var jwt =
            _jwtHelper.GenerateToken(user);

        string refreshToken =
            _jwtHelper.GenerateRefreshToken();

        DateTime refreshExpiry =
            DateTime.UtcNow.AddDays(30);

        await _repository.SaveRefreshTokenAsync(
            user.UserId,
            refreshToken,
            refreshExpiry);

        return new AuthResponse
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            AccessToken = jwt.Token,
            RefreshToken = refreshToken,
            Expiry = jwt.Expiry,
            IsNewUser = false
        };
    }

    #endregion

    #region Google Login

    public async Task<AuthResponse?> GoogleLoginAsync(
        GoogleLoginRequest request)
    {
        var googleUser =
            await _googleAuthService.VerifyGoogleTokenAsync(request.IdToken);

        if (googleUser == null)
            return null;

        bool isNewUser = false;

        var user =
            await _repository.GetUserByEmailAsync(googleUser.Email);

        if (user == null)
        {
            await _repository.RegisterGoogleUserAsync(googleUser);

            user =
                await _repository.GetUserByEmailAsync(googleUser.Email);

            if (user == null)
                return null;

            isNewUser = true;
        }

        if (string.IsNullOrWhiteSpace(user.GoogleId))
        {
            await _repository.LinkGoogleAccountAsync(
                user.UserId,
                googleUser.GoogleId,
                googleUser.Picture);

            user.GoogleId = googleUser.GoogleId;
        }

        await _repository.UpdateLastLoginAsync(user.UserId);

        var jwt =
            _jwtHelper.GenerateToken(user);

        string refreshToken =
            _jwtHelper.GenerateRefreshToken();

        DateTime refreshExpiry =
            DateTime.UtcNow.AddDays(30);

        await _repository.SaveRefreshTokenAsync(
            user.UserId,
            refreshToken,
            refreshExpiry);

        return new AuthResponse
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            AccessToken = jwt.Token,
            RefreshToken = refreshToken,
            Expiry = jwt.Expiry,
            IsNewUser = isNewUser
        };
    }


    public async Task<AuthResponse?> RefreshTokenAsync(
    RefreshTokenRequest request)
    {
        var refreshToken =
            await _repository.GetRefreshTokenAsync(request.RefreshToken);

        if (refreshToken == null)
            return null;

        if (refreshToken.IsRevoked)
            return null;

        if (refreshToken.ExpiryDate <= DateTime.UtcNow)
            return null;

        var user =
            await _repository.GetUserByIdAsync(refreshToken.UserId);

        if (user == null)
            return null;

        // Revoke old refresh token
        await _repository.RevokeRefreshTokenAsync(request.RefreshToken);

        // Generate new access token
        var jwt = _jwtHelper.GenerateToken(user);

        // Generate new refresh token
        string newRefreshToken =
            _jwtHelper.GenerateRefreshToken();

        DateTime refreshExpiry =
            DateTime.UtcNow.AddDays(30);

        // Save new refresh token
        await _repository.SaveRefreshTokenAsync(
            user.UserId,
            newRefreshToken,
            refreshExpiry);

        return new AuthResponse
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            AccessToken = jwt.Token,
            RefreshToken = newRefreshToken,
            Expiry = jwt.Expiry,
            IsNewUser = false
        };
    }


    public async Task<bool> LogoutAsync(
    LogoutRequest request)
    {
        bool valid =
            await _repository.IsRefreshTokenValidAsync(
                request.RefreshToken);

        if (!valid)
            return false;

        await _repository.RevokeRefreshTokenAsync(
            request.RefreshToken);

        return true;
    }

    public async Task<bool> ChangePasswordAsync(
    int userId,
  ChangePasswordRequest request)
    {
        var user =
            await _repository.GetUserByIdAsync(userId);

        if (user == null)
            return false;

        if (string.IsNullOrWhiteSpace(user.PasswordHash))
            return false;

        bool valid =
            PasswordHelper.VerifyPassword(
                request.CurrentPassword,
                user.PasswordHash);

        if (!valid)
            return false;

        string newHash =
            PasswordHelper.HashPassword(
                request.NewPassword);

        return await _repository.ChangePasswordAsync(
            userId,
            newHash);
    }


    public async Task<bool> ForgotPasswordAsync(
    ForgotPasswordRequest request)
    {
        var user =
            await _repository.GetUserByEmailAsync(request.Email);

        // Don't reveal whether the email exists
        if (user == null)
            return true;

        string token =
            _jwtHelper.GeneratePasswordResetToken();

        await _repository.SavePasswordResetTokenAsync(
            user.UserId,
            token,
            DateTime.UtcNow.AddMinutes(15));

        string resetUrl =
            $"{_configuration["Frontend:BaseUrl"]}/reset-password?token={Uri.EscapeDataString(token)}";

        string html =
            EmailTemplateHelper.LoadTemplate(
            "Templates/Emails/ForgotPassword.html");

        html =
            EmailTemplateHelper.ReplacePlaceholders(
            html,
            new Dictionary<string, string>
            {
            { "Name", user.FullName },
            { "ResetUrl", resetUrl }
            });

        await _emailService.SendAsync(
            user.Email,
            "Reset Your Password",
            html);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(
    ResetPasswordRequest request)
    {
        var token =
            await _repository.GetPasswordResetTokenAsync(
                request.Token);

        if (token == null)
            return false;

        if (token.IsUsed)
            return false;

        if (token.ExpiryDate < DateTime.UtcNow)
            return false;

        string hash =
            PasswordHelper.HashPassword(
                request.NewPassword);

        await _repository.UpdatePasswordAsync(
            token.UserId,
            hash);

        await _repository.MarkPasswordResetTokenUsedAsync(
            request.Token);

        return true;
    }

    #endregion
}