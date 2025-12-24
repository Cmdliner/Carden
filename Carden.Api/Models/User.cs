using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Models;

public class User
{
    [Key]
    public Guid Id { get; init; }
    
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [MaxLength(255, ErrorMessage = "Email length cannot exceed 255 characters")]
    public required string Email { get; set; }

    [MaxLength(50)]
    [Required]
    public required string Username { get; set; }
    
    [MaxLength(200)]
    public required string PasswordHash { get; set; }
    
    [MaxLength(200)]
    public string? ProfileImageUrl { get; set; }
    
    public DateTime EmailVerifiedAt { get; set; }
    
    public DateTime LastLogin { get; set; }
    
    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; set; }
}