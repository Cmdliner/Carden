using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error"},
                Success = false,
                Timestamp = DateTime.UtcNow
                });
        }
    }
    
    [HttpPut("profile/image")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    public async  Task<IActionResult> UpdateUserProfile([FromForm] IFormFile formFile)
    {
        try
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
        
            var result = await _userService.UploadProfileImage(Guid.Parse(userId), formFile);
            return result.ToActionResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error"},
                Success = false,
                Timestamp = DateTime.UtcNow
                });
        }
    }
    
    [HttpDelete("/{user_id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid user_id)
    {
        try
        {
            var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userId is null || Guid.Parse(userId) != user_id)
            {
                return Unauthorized(new ApiResponse
                {
                    Message = "Unauthorized",
                    Success = false,
                    Timestamp = DateTime.UtcNow
                });
            }

            return Ok(new { Success = true, message = "User deleted" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error"},
                Success = false,
                Timestamp = DateTime.UtcNow
                });
        }
    }
}