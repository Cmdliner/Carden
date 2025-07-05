using Carden.Api.Dtos;
using Carden.Api.Helpers;

namespace Carden.Api.Interfaces;

public interface IAuthService
{
    public  Task<Result<User>> Register(UserRegistrationDto userData);
    public  Task<Result<LoginResponseDto>> Login(UserLoginDto loginData);
    public  Task<Result<object>> Refresh();
    public  Task<Result<object>> Logout();
}