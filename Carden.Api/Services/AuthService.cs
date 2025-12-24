using Carden.Api.Controllers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Services;

public interface IAuthService
{
    Task<User> Register(RegisterDto registerDto);
    Task<ValueTuple<string, string>> Login(LoginDto loginDto);
    Task ForgotPassword(string email);
    Task<ValueTuple<bool, string>> VerifyPasswordReset(VerifyPasswordResetDto verifyPasswordResetDto);

    Task ResetPassword(string passwordResetToken, string newPassword);
}

public class AuthService(AppDbContext context, IPasswordHelper passwordHelper, IJwtHelper jwtHelper) : IAuthService
{
    private readonly AppDbContext _context = context;
    private readonly IPasswordHelper _passwordHelper = passwordHelper;
    private readonly IJwtHelper _jwtHelper = jwtHelper;

    public async Task<User> Register(RegisterDto registerDto)
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = _passwordHelper.GenerateHash(registerDto.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<(string, string)> Login(LoginDto loginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);
        if (user is null) throw new BadHttpRequestException("Invalid credentials");

        var passwordsMatch = _passwordHelper.Verify(loginDto.Password, user.PasswordHash);
        if (!passwordsMatch) throw new BadHttpRequestException("Invalid credentials");

        user.LastLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var accessToken = _jwtHelper.GenerateToken(user.Id);

        var refreshToken = new AuthToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Hash = _passwordHelper.GenerateHash(accessToken), // !todo => Update to use different hasher
            TokenType = TokenType.Refresh,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(30),
        };
        await _context.AuthTokens.AddAsync(refreshToken);

        await _context.SaveChangesAsync();

        return (accessToken, refreshToken.Hash);
    }

    public async Task ForgotPassword(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return;

        var newOtp = new Otp
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Kind = OtpKind.PasswordReset,
            Code = new Random().Next(100_000, 999_999).ToString(),
            ExpiresAt = new DateTime().AddMinutes(5).ToUniversalTime()
        };
        await _context.Otps.AddAsync(newOtp);
        await _context.SaveChangesAsync();

        // ! todo => Send email to user
        Console.WriteLine($"Otp => {newOtp.Code}");
    }

    public async Task<(bool, string)> VerifyPasswordReset(VerifyPasswordResetDto verifyPasswordResetDto)
    {
        var (email, code) = verifyPasswordResetDto;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null) return (false, string.Empty);

        var otp = await _context.Otps.FirstOrDefaultAsync(o => o.Code == code && o.UserId == user.Id);
        if (otp is null || otp.ExpiresAt < DateTime.UtcNow) return (false, string.Empty);

        var passwordResetToken = new AuthToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenType = TokenType.PasswordReset,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            Hash = _passwordHelper.GenerateHash(user.Id.ToString()),
        };
        await _context.AuthTokens.AddAsync(passwordResetToken);
        await _context.SaveChangesAsync();

        return (true, passwordResetToken.Hash);
    }

    public async Task ResetPassword(string passwordResetToken, string newPassword)
    {
        var dbToken = await _context.AuthTokens.FirstOrDefaultAsync(t => t.Hash == passwordResetToken);
        if (dbToken is null || dbToken.RevokedAtUtc is not null || dbToken.ExpiresAtUtc < DateTimeOffset.UtcNow)
        {
            throw new BadHttpRequestException("Invalid credentials");
        }
        
        dbToken.RevokedAtUtc =  DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == dbToken.UserId);
        if (user is null) throw new BadHttpRequestException("Invalid credentials");

        user.PasswordHash = _passwordHelper.GenerateHash(newPassword);
        await _context.SaveChangesAsync();
    }
}