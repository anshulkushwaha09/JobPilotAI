using JobPilot.API.DTOs.Profile;
using JobPilot.API.DTOs.Response;
using JobPilot.API.Helpers;
using JobPilot.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobPilot.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        int userId = UserContext.GetUserId(User);

        var profile =
            await _profileService.GetProfileAsync(userId);

        if (profile == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Profile not found."
            });
        }

        return Ok(new ApiResponse<ProfileResponse>
        {
            Success = true,
            Message = "Profile fetched successfully.",
            Data = profile
        });
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(
        UpdateProfileRequest request)
    {
        int userId = UserContext.GetUserId(User);

        bool updated =
            await _profileService.UpdateProfileAsync(
                userId,
                request);

        if (!updated)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Unable to update profile."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Profile updated successfully."
        });
    }
}