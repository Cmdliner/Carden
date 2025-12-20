using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Models;

public class Otp
{
    [Key]
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Please enter a valid Otp Token")]
    public required string Code { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [Required]
    public OtpKind Kind { get; set; }
    
    [Required]
    public DateTime ExpiresAt { get; set; }
}

public enum OtpKind
{
    PasswordReset,
    UserVerification,
}