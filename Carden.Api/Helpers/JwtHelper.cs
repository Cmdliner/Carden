using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Carden.Api.Helpers;

public interface IJwtHelper
{
    public string GenerateToken(Guid userId);
}

public class JwtHelper(IConfiguration config): IJwtHelper
{
    private readonly IConfiguration _config = config;

    public string GenerateToken(Guid userId)
    {
        var jwtSettings = _config.GetSection("Jwt");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var claims = new[]
        {
            new Claim("sub", userId.ToString()),
        };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: jwtSettings["issuer"],
            audience: jwtSettings["audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}