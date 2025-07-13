using System.Net;
using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Helpers;
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
        //
        // if (!ModelState.IsValid)
        // {
        //     var errors = ModelState.SelectMany(x =>
        //         x.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? []);
        //     return UnprocessableEntity(new { success = false, errors });                                                                              
        // }

        var result = await _authService.Register(userRegistrationDto);
        return result.ToCreatedResult();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var result = await _authService.Login(userLoginDto);
        if(result.IsSuccess) SetRefreshCookie(result.Value.RefreshToken.ToString());
        return result.ToActionResult();
    }
    
    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        if (userId is null) return Unauthorized("Invalid access token");

        var refreshToken = Request.Cookies["refresh"];
        if (refreshToken is null) return Unauthorized("Invalid refresh token");
        
        var result = await _authService.Refresh( userId,  refreshToken);
        return result.ToActionResult();
    }
    
    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshCookie = Request.Cookies["refresh"];

        var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
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