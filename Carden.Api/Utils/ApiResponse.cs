using System.Text.Json.Serialization;

namespace Carden.Api.Utils;

public class ApiResponse
{
    public ApiResponse() {}

    public ApiResponse(bool success, string mssg, ErrorDetails error)
    {
        Success = success;
        Message = mssg;
        Error = error;
    }

    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    [JsonPropertyName("error")]
    public ErrorDetails? Error { get; set; }
    
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public class ApiResponse<T> : ApiResponse
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}