using System.ComponentModel.DataAnnotations;

namespace Carden.Api.Entities;

public class User
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public required string Email { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string FullName { get; set; }
    public string? Username { get; set; }
    
    public required string PasswordHash { get; set; }
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; } = null;

    public DateTime? DeletedAt { get; set; } = null;
    // public string? ProfileImgUrl { get; set; }

}