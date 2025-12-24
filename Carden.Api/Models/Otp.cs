using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Models;

public class Otp
{
    [Key]
    public Guid Id { get; init; }
    
    [MaxLength(10)]
    public required string Code { get; init; }
    
    public Guid UserId { get; init; }
    
    public OtpKind Kind { get; init; }
    
    public DateTime ExpiresAt { get; init; }
}

public enum OtpKind
{
    PasswordReset,
    UserVerification,
}