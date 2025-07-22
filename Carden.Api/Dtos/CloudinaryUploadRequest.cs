using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record CloudinaryUploadRequest
{
    [JsonPropertyName("form_file")]
    public IFormFile FormFile { get; set; }
    
    [JsonPropertyName("folder")]
    public string? Folder { get; set; }
    
    [JsonPropertyName("tags")]
    public string? Tags { get; set; }
    
    [JsonPropertyName("auto_optimize")]
    public bool AutoOptimize { get; set; } = true;
}

