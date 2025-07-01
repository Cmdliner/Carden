using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Validations;
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
        if (!ModelState.IsValid)
        {
            var errors = ModelState.SelectMany(x =>
                x.Value?.Errors.Select(e => e.ErrorMessage).ToList() ?? []);
            return UnprocessableEntity(new { success = false, errors });                                                                              
        }

        var result = await _authService.Register(userRegistrationDto);
        return Created(nameof(Register), new { success = true, message = "User registered successfully" });
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