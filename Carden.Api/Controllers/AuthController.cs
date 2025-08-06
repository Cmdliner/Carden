using System.Net;
using System.Security.Claims;
using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Utils;
using Carden.Api.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        try
        {
            var result = await _authService.Register(userRegistrationDto);
            return result.ToCreatedResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
            {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error" },
                Success = false,
                Timestamp = DateTime.UtcNow
            });        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        try
        {
            var result = await _authService.Login(userLoginDto);
            if (result.IsSuccess) SetRefreshCookie(result.Value.RefreshToken.ToString());
            return result.ToActionResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
            {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error" },
                Success = false,
                Timestamp = DateTime.UtcNow
            });        }
    }

    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            // Get the userId through the "sub" claim directly
            var userId = User.FindFirst("sub")?.Value;
            if (userId is null) return Unauthorized("Invalid access token");

            var refreshToken = Request.Cookies["refresh"];
            if (refreshToken is null) return Unauthorized("Invalid refresh token");

            var result = await _authService.Refresh(userId, refreshToken);
            return result.ToActionResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
            {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error" },
                Success = false,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var refreshCookie = Request.Cookies["refresh"];

            var userId = User.FindFirst("sub")?.Value;
            if (userId is null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Error = new ErrorDetails
                    {
                        Code = "401",
                        Message = "User is logged out already"
                    }
                });
            }

            if (refreshCookie is null)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Error = new ErrorDetails
                    {
                        Code = "400",
                        Message = "Refresh token required"
                    }
                });
            }

            var result = await _authService.Logout(userId, refreshCookie);
            return result.ToNoContentResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
            {
                Error = new ErrorDetails { Code = "500", Message = "Internal server error" },
                Success = false,
                Timestamp = DateTime.UtcNow
            });
        }
    }


    private void SetRefreshCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(30),
            Path = "api/v1/auth/refresh"
        };

        Response.Cookies.Append("refresh", refreshToken, cookieOptions);
    }
}