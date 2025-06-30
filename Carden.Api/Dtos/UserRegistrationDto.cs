namespace Carden.Api.Dtos;

public record UserRegistrationDto
{
    public required string Email { get; set; }

    public required string FullName { get; set; }

    public string? Username { get; set; }
    
    public required string Password { get; set; }
}