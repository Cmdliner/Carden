using Carden.Api.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Services;

public interface IAuthService
{
    Task<User> Register(RegisterRequest registerRequest);
    Task<string> Login(LoginRequest loginRequest);
    Task RequestPasswordReset(string email);
    Task<bool> VerifyPasswordReset(VerifyPasswordResetRequest verifyPasswordResetRequest);

    Task ResetPassword(string passwordResetToken, string newPassword);
}

public class AuthService(AppDbContext context, IPasswordHelper passwordHelper) : IAuthService
{
    private readonly AppDbContext _context = context;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;

    public async Task<User> Register(RegisterRequest registerRequest)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = registerRequest.Username,
            Email = registerRequest.Email,
            PasswordHash = _passwordHelper.GenerateHash(registerRequest.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<string> Login(LoginRequest loginRequest)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
        if (user is null) throw new BadHttpRequestException("Invalid credentials");

        var passwordsMatch = _passwordHelper.Verify(loginRequest.Password, user.PasswordHash);
        if (!passwordsMatch) throw new BadHttpRequestException("Invalid credentials");

        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync(); 

        var authToken = string.Empty;
        return authToken;
    }

    public async Task RequestPasswordReset(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return;

        var newOtp = new Otp
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Kind = OtpKind.PasswordReset,
            Code = new Random().Next(100_000, 999_999).ToString(),
            ExpiresAt = new DateTime().AddMinutes(5)
        };
        await _context.Otps.AddAsync(newOtp);
        await _context.SaveChangesAsync();

        // ! todo => Send email to user
        Console.WriteLine($"Otp => {newOtp.Code}");
    }

    public async Task<bool> VerifyPasswordReset(VerifyPasswordResetRequest verifyPasswordResetRequest)
    {
        var (email, code) = verifyPasswordResetRequest;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return false;

        var otp = await _context.Otps.FirstOrDefaultAsync(o => o.Code == code && o.UserId == user.Id);
        return otp is not null || otp?.ExpiresAt > DateTime.UtcNow;
    }

    public async Task ResetPassword(string passwordResetToken, string newPassword)
    {
        // ! todo => Verify the password reset token and extract the userId from it
        var userId = Guid.NewGuid();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) throw new BadHttpRequestException("Invalid credentials");

        user.PasswordHash = _passwordHelper.GenerateHash(newPassword);
        await _context.SaveChangesAsync();
    }
}