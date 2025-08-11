using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record RegisterResponseDto : LoginResponseDto
{
}

public record LoginResponseDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public required Guid RefreshToken { get; set; }
    
    [JsonPropertyName("user")]
    public required string User { get; set; }
}

public record RefreshResponseDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
}