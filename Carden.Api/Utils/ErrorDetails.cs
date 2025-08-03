using System.Text.Json.Serialization;

namespace Carden.Api.Utils;

public class ErrorDetails
{
    public ErrorDetails() {}
    public ErrorDetails(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public ErrorDetails(string code, string message, string[] details): this(code, message)
    {
        Details = details.ToList();
    }

    

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    
    [JsonPropertyName("details")]
    public List<string>? Details { get; set; }
}