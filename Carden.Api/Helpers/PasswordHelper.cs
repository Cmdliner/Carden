﻿using Carden.Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Carden.Api.Helpers;

public static class PasswordHelper
{
    public static string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);

    }

    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    
}