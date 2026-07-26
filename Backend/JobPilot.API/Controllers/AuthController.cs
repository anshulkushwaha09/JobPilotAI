using JobPilot.API.DTOs.Request;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Helpers;
using JobPilot.API.Models.Request;
using JobPilot.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPilot.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        if (result == null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Registration Failed"
            });
        }

        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Registration Successful",
            Data = result
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid Email or Password"
            });
        }

        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Login Successful",
            Data = result
        });
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin(GoogleLoginRequest request)
    {
        var result = await _authService.GoogleLoginAsync(request);

        if (result == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Google Login Failed"
            });
        }

        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Google Login Successful",
            Data = result
        });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
    RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);

        if (result == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid or Expired Refresh Token"
            });
        }

        return Ok(new ApiResponse<AuthResponse>
        {
            Success = true,
            Message = "Token Refreshed Successfully",
            Data = result
        });
    }



    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
    LogoutRequest request)
    {
        bool result =
            await _authService.LogoutAsync(request);

        if (!result)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid Refresh Token"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Logout Successful"
        });
    }



    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
    ChangePasswordRequest request)
    {
        int userId =
            CurrentUserHelper.GetUserId(User);

        bool result =
            await _authService.ChangePasswordAsync(
                userId,
                request);

        if (!result)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Current Password Incorrect"
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Password Changed Successfully"
        });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(
    ForgotPasswordRequest request)
    {
        await _authService.ForgotPasswordAsync(request);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "If an account with that email exists, a password reset link has been sent."
        });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
    ResetPasswordRequest request)
    {
        bool success =
            await _authService.ResetPasswordAsync(request);

        if (!success)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Invalid or expired reset link."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Password reset successfully."
        });
    }
}