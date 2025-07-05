using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Helpers;
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
        return result.ToActionResult();
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