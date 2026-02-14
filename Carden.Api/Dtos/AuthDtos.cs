namespace Carden.Api.Dtos;

public record LoginDto(string Email, string Password);

public record RegisterDto(string Username, string Email, string Password);

public record ForgotPasswordDto(string Email);

public record VerifyPasswordResetDto(string Email, string Code);

public record ResetPasswordDto(string Password);