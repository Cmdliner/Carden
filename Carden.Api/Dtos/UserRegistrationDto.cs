﻿using System.Text.Json.Serialization;

namespace Carden.Api.Dtos;

public record UserRegistrationDto
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("full_name")]
    public required string FullName { get; set; }
    
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}