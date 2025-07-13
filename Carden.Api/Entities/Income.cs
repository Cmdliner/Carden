using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Carden.Api.Entities;

public class Income
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }
    
    [Required]
    public required string Source { get; set; }
    
    public string? Description { get; set; }

    public DateTime? ReceivedAt { get; set; }

    public virtual User User { get; set; } = null!;
}