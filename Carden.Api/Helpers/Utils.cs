using Carden.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Carden.Api.Helpers;

public static class Utils
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);

    }

    public static bool VerifyPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public static string GenerateJwt(string payload, string secret)
    {
        return string.Empty;
    }
}