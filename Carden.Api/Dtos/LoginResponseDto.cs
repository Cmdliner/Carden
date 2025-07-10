using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record LoginResponseDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
    
    [JsonPropertyName("refresh_token")]
    public required Guid RefreshToken { get; set; }
}

public record RefreshResponseDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
}