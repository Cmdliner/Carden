using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Repositories;

interface IWalletRepository
{
    public Task<Guid?> CreateAsync(Wallet wallet);
    public Task<Wallet?> FindAsync(Guid id);
    public Task<Wallet?> UpdateAsync(Wallet wallet);
}

public class WalletRepository(ApplicationDbContext context): IWalletRepository
{
    
    private readonly ApplicationDbContext _context = context;
    
    public async Task<Guid?> CreateAsync(Wallet wallet)
    {
        try
        {
             _context.Wallets.Add(wallet);
             await _context.SaveChangesAsync();
             return wallet.Id;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<Wallet?> FindAsync(Guid id)
    {
        try
        {
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);
            return wallet;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<Wallet?> UpdateAsync(Wallet wallet)
    {
        try
        {
            _context.Wallets.Update(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}