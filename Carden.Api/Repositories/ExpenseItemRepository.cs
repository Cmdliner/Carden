using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Repositories;

public interface IExpenseItemRepository
{
    public Task<ExpenseItem?> FindAsync(Guid item_id);

    public Task<ExpenseItem> AddItemAsync(ExpenseItem expenseItem, uint? priority = null);
    public Task<List<ExpenseItem>> FindByUser(Guid userId, uint? take = null);
    public Task<ExpenseItem?> UpdatePriorityAsync(Guid itemId, uint priority);
    public Task<bool> DeleteAsync(Guid item_id);
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
            var maxPriority = await _context.ExpenseItems
                .Where(e => e.UserId == expenseItem.UserId)
                .Select(e => (uint?)e.Priority)
                .MaxAsync() ?? 0;
            uint targetPriority;

            if (priority.HasValue)
            {
                if (priority.Value > maxPriority + 1) targetPriority = maxPriority + 1;
                else
                {
                    targetPriority = priority.Value;

                    // If inserting in the middle, shift other items up
                    if (targetPriority <= maxPriority)
                    {
                        await _context.ExpenseItems
                            .Where(e => e.UserId == expenseItem.UserId && e.Priority >= targetPriority)
                            .ExecuteUpdateAsync(setters => setters
                                .SetProperty(e => e.Priority, e => e.Priority + 1));
                    }
                }
            }
            else targetPriority = maxPriority + 1;


            expenseItem.Priority = targetPriority;
            _context.ExpenseItems.Add(expenseItem);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return expenseItem;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ExpenseItem?> UpdatePriorityAsync(Guid itemId, uint priority)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var item = await _context.ExpenseItems.FirstOrDefaultAsync(e => e.Id == itemId);
            if (item is null) return null;

            var maxPriority = await _context.ExpenseItems
                .Where(e => e.UserId == item.UserId)
                .MaxAsync(e => e.Priority);

            if (priority > maxPriority) return null;

            var oldPriority = item.Priority;
            var newPriority = priority;

            if (oldPriority == newPriority) return item;

            if (newPriority > oldPriority)
            {
                // Moving down: decrease priority of items between old and new position
                await _context.ExpenseItems
                    .Where(e => e.UserId == item.UserId &&
                                e.Priority > oldPriority &&
                                e.Priority <= newPriority)
                    .OrderBy(e => e.Priority)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(e => e.Priority, e => e.Priority - 1));
            }
            else
            {
                // Moving up: increase priority of items between new and old position
                await _context.ExpenseItems
                    .Where(e => e.UserId == item.UserId &&
                                e.Priority >= newPriority &&
                                e.Priority < oldPriority)
                    .OrderBy(e => e.Priority)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(e => e.Priority, e => e.Priority + 1))
                    
                    ;
            }

            // item.Priority = newPriority;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return item;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid item_id)
    {
        try
        {
            var expenseItem = await _context.ExpenseItems.FirstOrDefaultAsync(i => i.Id == item_id);
            if (expenseItem is null) return false;
            
             _context.ExpenseItems.Remove(expenseItem);
             await _context.SaveChangesAsync();

             return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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