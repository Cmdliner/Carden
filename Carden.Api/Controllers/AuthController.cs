using Microsoft.AspNetCore.Identity.Data;

namespace Carden.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var user = await _authService.Register(registerRequest);
        return Created(nameof(Register), new { Success = true, Message = "User created successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        try
        {
            var authToken = await _authService.Login(loginRequest);
            return Ok(new { Success = true, Message = "User login successfully", AuthToken = authToken });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> RequestPasswordReset(string email)
    {
        await _authService.RequestPasswordReset(email);
        return Ok(new { Success = true, Message = "Password reset instructions have been snt to your email" });
    }

    [HttpPost("verify-password-reset")]
    public async Task<IActionResult> VerifyPasswordReset(VerifyPasswordResetRequest verifyPasswordResetRequest)
    {
        var isValidOtp = await _authService.VerifyPasswordReset(verifyPasswordResetRequest);
        if (!isValidOtp) return BadRequest(new { Success = false, Message = "Invalid Otp" });

        return Ok(new { Success = true, Message = "Otp verified", OtpToken = string.Empty });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
    {
        try
        {
            var passwordResetToken = HttpContext.Request.Headers["X-Password-Reset"].ToString();

            await _authService.ResetPassword(passwordResetToken, resetPasswordRequest.Password);

            return Ok(new { Success = true, Message = "Password has been reset successfully" });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }
}

public record LoginRequest(string Email, string Password);

public record RegisterRequest(string Username, string Email, string Password);

public record VerifyPasswordResetRequest(string Email, string Code);

public record ResetPasswordRequest(string Password);