using Carden.Api.Dtos;
using Carden.Api.Entities;
using Carden.Api.Helpers;
using Carden.Api.Interfaces;
using Carden.Api.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Carden.Api.Services;

public class AuthService(UserRepository userRepository): IAuthService
{

    private readonly UserRepository _userRepository = userRepository;
    public async Task<User> Register(UserRegistrationDto userData)
    {
        var emailAvailable = await _userRepository.FindByEmail(userData.Email) ?? throw new Exception("Email taken");
        
        var user = new User
        {
            Email = userData.Email,
            FullName = userData.FullName,
            Username = userData.Username,
            PasswordHash = Utils.HashPassword(userData.Password)
        };
        var registeredUser = await _userRepository.Create(user);
        return registeredUser;

    }

    public Task<object> Login()
    {
        throw new NotImplementedException();
    }

    public Task<object> Refresh()
    {
        throw new NotImplementedException();
    }

    public Task<object> Logout()
    {
        throw new NotImplementedException();
    }
}