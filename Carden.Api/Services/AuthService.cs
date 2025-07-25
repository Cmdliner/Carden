﻿using Carden.Api.Dtos;
using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IAuthService
{
    public  Task<Result<User>> Register(UserRegistrationDto userData);
    public  Task<Result<LoginResponseDto>> Login(UserLoginDto loginData);
    public  Task<Result<RefreshResponseDto>> Refresh(string userId, string refreshId);
    public  Task<Result<object>> Logout(string jwt, string refreshToken);
}

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    JwtHelper jwtHelper
) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    private readonly JwtHelper _jwtHelper = jwtHelper;

    public async Task<Result<User>> Register(UserRegistrationDto userData)
    {
        var emailAvailable = await _userRepository.FindByEmail(userData.Email);

        if (emailAvailable is not null) return Result.Failure<User>(Error.Conflict("Email taken!"));

        var user = new User
        {
            Email = userData.Email,
            FullName = userData.FullName,
            Username = userData.Username,
            PasswordHash = PasswordHelper.Hash(userData.Password)
        };
        var registeredUser = await _userRepository.Create(user);
        return Result.Success(registeredUser);
    }
    
    public async Task<Result<LoginResponseDto>> Login(UserLoginDto loginData)
    {
        var userInDb = await _userRepository.FindByEmail(loginData.Email);
        if (userInDb is null) return Result.Failure<LoginResponseDto>(Error.Forbidden("Invalid email or password"));

        var passwordsMatch = PasswordHelper.Verify(loginData.Password, userInDb.PasswordHash);
        if (!passwordsMatch) return Result.Failure<LoginResponseDto>(Error.Forbidden("Invalid email or password"));
        
        var accessToken = _jwtHelper.GenerateAccessToken(userInDb.Id);
        var refreshToken = await _refreshTokenRepository.Create(new RefreshToken { UserId = userInDb.Id });

        // Update lastLogin of user object in memory then saves to db using the repository
        userInDb.LastLogin = DateTime.UtcNow;
        await _userRepository.Update(userInDb);
        return Result.Success(new LoginResponseDto { AccessToken = accessToken, RefreshToken = refreshToken.Id });
    }

    
    public async Task<Result<RefreshResponseDto>> Refresh(string userId, string refreshId)
    {
        var dbRefreshToken = await _refreshTokenRepository.FindById(Guid.Parse(refreshId));
        if (dbRefreshToken is null) return Result.Failure<RefreshResponseDto>(Error.Unauthorized("Invalid refresh token"));

        var canRefresh = dbRefreshToken.RevokedAt is null && dbRefreshToken.ExpiresAt > DateTime.UtcNow && dbRefreshToken.UserId == Guid.Parse(userId);
        if (!canRefresh) return Result.Failure<RefreshResponseDto>(Error.Unauthorized("Refresh expired"));
        var accessToken = _jwtHelper.GenerateAccessToken(Guid.Parse(userId));
        return Result.Success(new RefreshResponseDto { AccessToken = accessToken });
    }
    
    public async Task<Result<object>> Logout(string userId, string refreshToken)
    {
        
        var deletedId = await _refreshTokenRepository
            .Delete(Guid.Parse(userId), Guid.Parse(refreshToken));
        return deletedId is null ? Result.Failure(Error.BadRequest("Error deleting refresh")) : Result.Success();
    }
    
}