using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Repositories;

public interface IExpenseItemRepository
{
    public Task<ExpenseItem> FindAsync(Guid item_id);

    public Task<ExpenseItem> CreateAsync(ExpenseItem expenseItem);
    public Task<List<ExpenseItem>> FindByUser(Guid userId, uint? take = null);
}

public class ExpenseItemRepository(ApplicationDbContext context): IExpenseItemRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ExpenseItem?> FindAsync(Guid item_id)
    {
        try
        {
            return await  _context.ExpenseItems.FirstOrDefaultAsync(e => e.Id == item_id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<ExpenseItem> CreateAsync(ExpenseItem expenseItem)
    {
        try
        {
             _context.ExpenseItems.Add(expenseItem);
            await _context.SaveChangesAsync();
            return expenseItem;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw e;
        }
    }

    public async Task<List<ExpenseItem>> FindByUser(Guid userId, uint? take = null)
    {
        try
        {
            List<ExpenseItem> expenseItems = [];
            
            var query = _context.ExpenseItems.Where(e => e.UserId == userId);
            
            expenseItems =  (take is null) ? query.ToList() : query.Take((int)take).ToList();
            return expenseItems;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
    } 
}