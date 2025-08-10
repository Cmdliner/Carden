using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Wallet> Wallets => Set<Wallet>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<ExpenseItem> ExpenseItems => Set<ExpenseItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity
                .HasIndex(u => u.Email)
                .IsUnique();
        });
            

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity
                .HasOne(r => r.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(r => r.UserId);
        });
            

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity
                .Property(w => w.Provider)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnType("varchar(20)");

            entity
                .HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId);
        });

        modelBuilder.Entity<ExpenseItem>(entity =>
        {
            entity
                .HasOne(e => e.User)
                .WithMany(u => u.ExpenseItems)
                .HasForeignKey(e => e.UserId);

            
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity
                .HasOne(i => i.User)
                .WithMany(u => u.IncomeList)
                .HasForeignKey(i => i.UserId);
        });
    }
}