using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context = context;


    public async Task<RefreshToken> Create(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }

    public async Task<RefreshToken?> FindById(Guid id)
    {
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Id == id);
        return refreshToken;
    }

    public async Task<RefreshToken[]> FindByUser(Guid id)
    {
        var tokens = await _context.RefreshTokens.Where(r => r.UserId == id)
            .ToArrayAsync();
        return tokens;
    }

    public async Task<RefreshToken?> Revoke(Guid id)
    {
        var refreshToken = _context.RefreshTokens.FirstOrDefault(r => r.Id == id);
        if (refreshToken is null) return null;

        refreshToken.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<Guid?> Delete(Guid userId, Guid refreshId)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Id == refreshId && r.UserId == userId);
        if (refreshToken is null) return null;
        
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken.Id;
    }
}