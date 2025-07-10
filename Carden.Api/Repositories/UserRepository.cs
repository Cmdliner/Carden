using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Repositories;

public class UserRepository(ApplicationDbContext context): IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<User> Create(User user)
    {
         _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User?> FindByEmail(string email)
    {
        var user  = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.DeletedAt == null);
        return user;
    }

    public async Task<User?> FindById(Guid userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId  && u.DeletedAt == null);
    }

    public async Task<User?> Update(User user)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id  && u.DeletedAt == null);
        if (userToUpdate is null) return null;

        userToUpdate = user;
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Guid?> Delete(Guid userId)
    {
        var userToDelete = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId  && u.DeletedAt == null);
        if (userToDelete is null) return null;
        
        userToDelete.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return userToDelete.Id;
    }
}