using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{

    private readonly AuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userRegistrationDto)
    {
        var result = await _authService.Register(userRegistrationDto);
        return Created(nameof(Register), new { message = "User registered successfully"});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        var result = await _authService.Login();
        return Ok(Guid.NewGuid());
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var result = await _authService.Refresh();
        return Ok(Guid.NewGuid());
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.Logout();
        return NoContent();
    }

}