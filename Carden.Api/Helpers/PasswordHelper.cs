namespace Carden.Api.Helpers;

public interface IPasswordHelper
{
    string GenerateHash(string password);
    bool Verify(string raw, string hash);
}

public class PasswordHelper: IPasswordHelper
{
    public string GenerateHash(string rawPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(rawPassword);
    }

    public bool Verify(string raw, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(raw, hash);
    }
}