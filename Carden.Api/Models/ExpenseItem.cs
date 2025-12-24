using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Models;


public class ExpenseItem
{
    [Key]
    public Guid Id { get; init; }
    
    public Guid UserId { get; init; }
    
    [MaxLength(50)]
    public required string Name { get; init; }
    
    [MaxLength(200)]
    public string? Description { get; set; }
    
    public long Amount { get; set; }
    
    public double Priority { get; set; }
    
}