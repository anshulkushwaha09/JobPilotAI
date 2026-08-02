using JobPilot.API.DTOs.Response;
using JobPilot.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace JobPilot.API.Controllers;


[Authorize]
[ApiController]
[Route("api/resume")]
public class ResumeController : ControllerBase
{

    private readonly IResumeService _service;


    public ResumeController(
    IResumeService service)
    {
        _service = service;
    }



    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
    IFormFile file,
    bool isDefault = false)
    {

        int userId =
        Convert.ToInt32(
        User.FindFirst("UserId")?.Value);



        var result =
        await _service.UploadAsync(
        userId,
        file,
        isDefault);



        return Ok(result);

    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {

        int userId =
        Convert.ToInt32(
        User.FindFirst("UserId")?.Value);


        return Ok(
        await _service.GetAllAsync(userId));

    }




    [HttpPut("default/{id}")]
    public async Task<IActionResult> Default(long id)
    {

        int userId =
        Convert.ToInt32(
        User.FindFirst("UserId")?.Value);



        return Ok(
        await _service.SetDefaultAsync(id, userId));

    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {

        int userId =
        Convert.ToInt32(
        User.FindFirst("UserId")?.Value);



        return Ok(
        await _service.DeleteAsync(id, userId));

    }


}