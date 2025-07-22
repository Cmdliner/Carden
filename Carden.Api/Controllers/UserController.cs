using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("/api/v{v:apiVersion}/[controller]s")]
[Authorize]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        foreach (var claim in User.Claims)
        {
            Console.WriteLine($"Claim => {claim}");
        }
        if (userId is null)
        {
            return Unauthorized(new ApiResponse
            {
                Message = "Unauthorized",
                Success = false,
                Timestamp = DateTime.UtcNow
            });
        }

        var result = await _userService.GetUser(Guid.Parse(userId));
        return result.ToActionResult();

    }


    [HttpPut("profile/image")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async  Task<IActionResult> UpdateUserProfile([FromForm] CloudinaryUploadRequest uploadRequest)
    {
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (userId is null)
        {
            return Unauthorized(new ApiResponse
            {
                Message = "Unauthorized",
                Success = false,
                Timestamp = DateTime.UtcNow
            });
        }
        
        var result = await _userService.UploadProfileImage(Guid.Parse(userId), uploadRequest);
        return result.ToActionResult();
    }
}