using System.ComponentModel.DataAnnotations;

namespace Carden.Api.Models;

public class User
{
    [Key]
    public Guid Id { get; init; }
    
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [MaxLength(255, ErrorMessage = "Email length cannot exceed 255 characters")]
    [Required]
    public required string Email { get; set; }

    [MaxLength(50)]
    [Required]
    public required string Username { get; set; }
    
    [MaxLength(200)]
    [Required]
    public required string PasswordHash { get; set; }
    
    public DateTime EmailVerifiedAt { get; set; }
    
    public DateTime LastLogin { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}