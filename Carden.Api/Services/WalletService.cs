using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IWalletService
{
    public Task<Result<Wallet>> CreateAsync();
    public Task<Result<Wallet?>> FindAsync();
    public Task<Result<Wallet?>> UpdateAsync();
    
}

public class WalletService(IWalletRepository walletRepository) : IWalletService
{
    private readonly IWalletRepository _walletRepository = walletRepository;

    public Task<Result<Wallet>> CreateAsync()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    public Task<Result<Wallet?>> FindAsync()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<Result<Wallet?>> UpdateAsync()
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }
}