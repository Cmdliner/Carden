using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record AddExpenseItemDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("category")]
    public string? Category { get; set; }
    
    [JsonPropertyName("expected_price")]
    public decimal ExpectedPrice { get; set; }
    
    [JsonPropertyName("priority")]
    public uint Priority { get; set; }
}