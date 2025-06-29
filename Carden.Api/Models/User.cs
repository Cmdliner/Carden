using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Carden.Api.Models;

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
    
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    // public string? ProfileImgUrl { get; set; }

}