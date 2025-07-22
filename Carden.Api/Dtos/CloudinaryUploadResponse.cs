using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record CloudinaryUploadResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }
    
    [JsonPropertyName("public_id")]
    public string? PublicId { get; set; }
    
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("format")]
    public string? Format { get; set; }
    
    [JsonPropertyName("bytes")]
    public long Bytes { get; set; }
    
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }
}