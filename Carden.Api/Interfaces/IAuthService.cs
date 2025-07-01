using Carden.Api.Dtos;

namespace Carden.Api.Interfaces;

public interface IAuthService
{
    public  Task<User> Register(UserRegistrationDto userData);
    public  Task<object> Login();
    public  Task<object> Refresh();
    public  Task<object> Logout();
    // public  Task<object> Register();
}