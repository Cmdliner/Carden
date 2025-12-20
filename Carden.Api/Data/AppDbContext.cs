using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Otp> Otps { get; set; }
}