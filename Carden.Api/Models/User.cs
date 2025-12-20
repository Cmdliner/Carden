using System.ComponentModel.DataAnnotations;

namespace Carden.Api.Models;

public class User
{
    public Guid Id { get; init; }
    
    [MaxLength(50)]
    public required string Email { get; set; }

    [MaxLength(50)]
    public required string Username { get; set; }
    
    [MaxLength(200)]
    public required string PasswordHash { get; set; }
    
    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}