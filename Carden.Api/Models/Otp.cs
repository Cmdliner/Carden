using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Models;

public class Otp
{
    [Key]
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = "Please enter a valid Otp Token")]
    [MaxLength(10)]
    public required string Code { get; init; }
    
    [Required]
    public Guid UserId { get; init; }
    
    [Required]
    public OtpKind Kind { get; init; }
    
    [Required]
    public DateTime ExpiresAt { get; init; }
}

public enum OtpKind
{
    PasswordReset,
    UserVerification,
}