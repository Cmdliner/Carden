namespace Carden.Api.Models;

public class Otp
{
    public Guid Id { get; set; }
    
    public required string Code { get; set; }

    public Guid UserId { get; set; }
    
    public OtpKind Kind { get; set; }
    
    public DateTime ExpiresAt { get; set; }
}

public enum OtpKind
{
    PasswordReset,
    UserVerification,
}