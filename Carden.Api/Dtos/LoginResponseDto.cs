using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record LoginResponseDto
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; }
}