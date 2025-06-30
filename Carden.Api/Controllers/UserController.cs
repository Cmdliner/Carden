using Asp.Versioning;
using Carden.Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("/api/v{v:apiVersion}/[controller]s")]
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
            UpdatedAt = DateTime.UtcNow,
            PasswordHash = "dsguiuduigudto"
        };
        return user;
    }
}