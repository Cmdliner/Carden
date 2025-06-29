using Carden.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    [HttpGet]
    public User GetUserProfile()
    {
        User user = new()
        {
            Username = "Example",
            FullName = "Abiade Abdulazeez",
            Email = "example@me.com",
            LastLogin = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return user;
    }
}