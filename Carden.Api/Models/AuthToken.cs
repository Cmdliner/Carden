using System.ComponentModel.DataAnnotations;

namespace Carden.Api.Models;

public class AuthToken
{
    [Key]
    public Guid Id { get; init; }

    public Guid UserId { get; init; }
    
    [MaxLength(200)]
    public required string Hash { get; init; }
    
    public TokenType TokenType { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; }
    
    public DateTimeOffset ExpiresAtUtc { get; init; }

    public DateTimeOffset? RevokedAtUtc { get; set; }
}

public enum TokenType
{
    ApiKey,
    Refresh,
    PasswordReset,
}