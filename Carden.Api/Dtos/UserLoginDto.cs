using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record UserLoginDto
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }
    
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}