using Microsoft.EntityFrameworkCore;

namespace Carden.Api.Services;

public interface IExpenseItemService
{
    public Task AddExpenseItem(AddExpenseItemDto dto, Guid userId);
    public Task RemoveExpenseItem(Guid expenseItemId, Guid userId);
    public Task UpdatePriority(Guid expenseItemId, Guid userId, double priority);
    public Task<List<ExpenseItem>> GetExpenseItems(Guid userId);
}

public class ExpenseItemService(AppDbContext context) : IExpenseItemService
{
    private readonly AppDbContext _context = context;

    public async Task AddExpenseItem(AddExpenseItemDto dto, Guid userId)
    {
        var currentMaxPriority = await _context.ExpenseItems.
            Where(e => e.UserId == userId).
            MaxAsync(e => e.Priority);
        var priority = dto.Priority ?? 0L;

        if (priority > 0 && priority < currentMaxPriority)
        {
            // If there is a priority in the dto request body
            // If the priority is in between other priorities ensure it is given a larger decimal place
        }
        else
        {
            priority = currentMaxPriority + 1;
        }

        var expenseItem = new ExpenseItem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = dto.Name,
            Description = dto.Description,
            Amount = dto.Amount,
            Priority = priority
        };
        await _context.ExpenseItems.AddAsync(expenseItem);
    }

    public async Task RemoveExpenseItem(Guid expenseItemId, Guid userId)
    {
        var expenseItem = await _context.ExpenseItems
            .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == expenseItemId);
        if (expenseItem is null) throw new BadHttpRequestException("Item not found");

        _context.ExpenseItems.Remove(expenseItem);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePriority(Guid expenseItemId, Guid userId, double priority)
    {
        var expenseItem = await _context.ExpenseItems
            .FirstOrDefaultAsync(e => e.UserId == userId && e.Id == expenseItemId);
        if (expenseItem is null) throw new BadHttpRequestException("Item not found");

        expenseItem.Priority = priority;
        await _context.SaveChangesAsync();
    }

    public async Task<List<ExpenseItem>> GetExpenseItems(Guid userId)
    {
        return await _context.ExpenseItems
            .Where(e => e.UserId == userId)
            .OrderBy(e => e.Priority)
            .ToListAsync();
    }
}