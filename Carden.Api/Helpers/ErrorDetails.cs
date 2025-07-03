using System.Text.Json.Serialization;

namespace Carden.Api.Helpers;

public class ErrorDetails
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("details")]
    public List<string>? Details { get; set; }
}