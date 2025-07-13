using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carden.Api.Entities;

public class RefreshToken
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();
    
    [Required]
    public Guid UserId { get; init; }
    
    [Required]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Required]
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(30);

    public DateTime? RevokedAt { get; set; }

    public virtual User User { get; set; } = null!;

}