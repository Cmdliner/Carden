using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carden.Api.Entities;

public class Wallet
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid UserId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Balance { get; set; } = 0M;
    
    [Required]
    public WalletProvider Provider { get; set; } = WalletProvider.Paystack;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = null!;

}

public enum WalletProvider
{
    Paystack
}