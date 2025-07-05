using Carden.Api.Dtos;
using Carden.Api.Helpers;

namespace Carden.Api.Services;

public class AuthService(IUserRepository userRepository, JwtHelper jwtHelper): IAuthService
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtHelper _jwtHelper = jwtHelper;
    
    public async Task<Result<User>> Register(UserRegistrationDto userData)
    {
        var emailAvailable = await _userRepository.FindByEmail(userData.Email);
        
        if(emailAvailable is not null) return Result.Failure<User>(Error.Conflict("Email taken!"));
        
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
        if(!passwordsMatch) return Result.Failure<LoginResponseDto>(Error.Forbidden("Invalid email or password"));
        
        var accessToken = jwtHelper.GenerateAccessToken(userInDb.Id);
        return Result.Success<LoginResponseDto>(new LoginResponseDto { AccessToken = accessToken });
    }

    public Task<Result<object>> Refresh()
    {
        throw new NotImplementedException();
    }

    public Task<Result<object>> Logout()
    {
        throw new NotImplementedException();
    }
}