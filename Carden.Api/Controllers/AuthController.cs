using Microsoft.AspNetCore.Identity.Data;

namespace Carden.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var user = await _authService.Register(registerDto);
        return Created(nameof(Register), new { Success = true, Message = "User created successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        try
        {
            var (accessToken, refreshToken) = await _authService.Login(loginDto);
            return Ok(new
            {
                Success = true,
                Message = "User login successfully",
                Data = new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }
            });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        await _authService.ForgotPassword(forgotPasswordDto.Email);
        return Ok(new { Success = true, Message = "Password reset instructions have been snt to your email" });
    }

    [HttpPost("verify-password-reset")]
    public async Task<IActionResult> VerifyPasswordReset(VerifyPasswordResetDto verifyPasswordResetDto)
    {
        var (isValidOtp, passwordResetHash) = await _authService.VerifyPasswordReset(verifyPasswordResetDto);
        if (!isValidOtp) return BadRequest(new { Success = false, Message = "Invalid Otp" });

        return Ok(new { Success = true, Message = "Otp verified", OtpToken = passwordResetHash });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            var passwordResetToken = HttpContext.Request.Headers["X-Password-Reset"].ToString();

            await _authService.ResetPassword(passwordResetToken, resetPasswordDto.Password);

            return Ok(new { Success = true, Message = "Password has been reset successfully" });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }
}

