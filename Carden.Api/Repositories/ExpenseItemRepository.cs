using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Repositories;

public interface IExpenseItemRepository
{
    public Task<ExpenseItem> FindAsync(Guid item_id);

    public Task<ExpenseItem> AddItemAsync(ExpenseItem expenseItem, uint? priority = null);
    public Task<List<ExpenseItem>> FindByUser(Guid userId, uint? take = null);
    public Task<ExpenseItem?> UpdatePriorityAsync(Guid itemId, uint priority);
}

public class ExpenseItemRepository(ApplicationDbContext context) : IExpenseItemRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ExpenseItem?> FindAsync(Guid item_id)
    {
        try
        {
            return await _context.ExpenseItems.FirstOrDefaultAsync(e => e.Id == item_id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<ExpenseItem> AddItemAsync(ExpenseItem expenseItem, uint? priority = null)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var targetPriority = priority ?? 1;
            var maxPriority = await _context.ExpenseItems.MaxAsync(e => e.Priority);
            if (targetPriority > maxPriority + 1) targetPriority = maxPriority + 1;

            #region REPRIORITIZE_EXPENSE_ITEM_QUERY
            await _context.ExpenseItems.Where(e => e.Priority >= targetPriority)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(e => e.Priority, e => e.Priority + 1));

            #endregion

            expenseItem.Priority = targetPriority;
            _context.ExpenseItems.Add(expenseItem);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return expenseItem;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw e;
        }
    }

    public async Task<ExpenseItem?> UpdatePriorityAsync(Guid itemId, uint priority)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var maxPriority = await _context.ExpenseItems.MaxAsync(e => e.Priority);
            if (priority > maxPriority) return null;

            var item = await _context.ExpenseItems.FirstOrDefaultAsync(e => e.Id == itemId);

            var oldPriority = item.Priority;
            var newPriority = priority;

            switch (newPriority)
            {
                case uint _ when newPriority > oldPriority:

                    // old ->  3  new -> 5 ( from 4 -> 5 priority - 1)
                    await _context.ExpenseItems
                        .Where(e => e.Priority > oldPriority && e.Priority <= newPriority)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(e => e.Priority, e => e.Priority - 1));
                    break;
                case uint _ when newPriority < oldPriority:
                    // old ->  5  new -> 3 ( affects 3 && 4, priority + 1)
                    await _context.ExpenseItems
                        .Where(e => e.Priority >= newPriority && e.Priority < oldPriority)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(e => e.Priority, e => e.Priority + 1));
                    break;
                default: return null;
            }

            item.Priority = newPriority;
            await _context.Database.CommitTransactionAsync();

            return item;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return null;
    }

    public async Task<List<ExpenseItem>> FindByUser(Guid userId, uint? take = null)
    {
        try
        {
            List<ExpenseItem> expenseItems = [];

            var query = _context
                .ExpenseItems
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.Priority);

            expenseItems = (take is null) ? query.ToList() : query.Take((int)take).ToList();
            return expenseItems;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return [];
        }
    }
}