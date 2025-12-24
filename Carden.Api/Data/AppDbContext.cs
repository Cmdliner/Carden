using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<AuthToken> AuthTokens { get; set; }

    public DbSet<Otp> Otps { get; set; }

    public DbSet<ExpenseItem> ExpenseItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<AuthToken>().HasIndex(t => t.Hash).IsUnique();
        modelBuilder.Entity<Otp>()
            .Property(o => o.Kind)
            .HasConversion<string>()
            .HasMaxLength(25);
        
        modelBuilder.Entity<ExpenseItem>().HasIndex( e => e.Priority);
        modelBuilder.Entity<ExpenseItem>().HasIndex(e => e.UserId);
    }
}