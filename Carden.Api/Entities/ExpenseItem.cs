using System.ComponentModel.DataAnnotations;

namespace Carden.Api.Entities;

public class ExpenseItem
{
    [Key]
    public Guid Id { get; init; }
    
    [Required]
    public Guid UserId { get; init; }
    
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    
    [MaxLength(50)]
    public string? Category { get; set; } = null;
    
    [Required]
    public decimal ExpectedPrice { get; set; }
    
    [Required]
    public uint Priority { get; set; }

    public DateTime? PurchasedAt { get; set; } = null;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public virtual User User { get; set; } = null!;

}