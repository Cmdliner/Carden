using Carden.Api.Dtos;
using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IExpenseItemService
{
    public Task<Result<ExpenseItem>> AddItem(Guid userId, AddExpenseItemDto expenseItemDto);
    public Task<Result<ExpenseItem>> GetItem(Guid itemId, Guid userId);
    public Task<Result<List<ExpenseItem>>> GetItems(Guid userId);
    public Task<Result<ExpenseItem>> UpdateItemPriority(Guid userId, Guid itemId, uint priority);
    public Task<Result<ExpenseItem>> UpdateItem(Guid userId);
    public Task<Result<string>> DeleteItem(Guid userId, Guid itemId);

}

public class ExpenseItemService(IExpenseItemRepository expenseItemRepository): IExpenseItemService
{

    private readonly IExpenseItemRepository _expenseItemRepository = expenseItemRepository;

    public async Task<Result<ExpenseItem>> AddItem(Guid userId,  AddExpenseItemDto expenseItemDto)
    {
        var expenseItem = new ExpenseItem
        {
            UserId = userId,
            Name = expenseItemDto.Name,
            Category = expenseItemDto.Category,
            Description = expenseItemDto.Description,
            ExpectedPrice = expenseItemDto.ExpectedPrice
        };
        
        var expenseItemInDb = await _expenseItemRepository.AddItemAsync(expenseItem, expenseItemDto.Priority);

        return Result.Success(expenseItemInDb);
    }

    public async Task<Result<ExpenseItem>> GetItem(Guid itemId, Guid userId)
    {
        try
        {
            var expenseItem = await _expenseItemRepository.FindAsync(itemId);
            if (expenseItem is null) return Result.Failure<ExpenseItem>(Error.BadRequest("expense item not found"));

            if (expenseItem.UserId != userId)
            {
                return Result.Failure<ExpenseItem>(Error.BadRequest("expense item not found!"));
            }

            return Result.Success(expenseItem);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<List<ExpenseItem>>> GetItems(Guid userId)
    {
        try
        {
            var expenseItems = await _expenseItemRepository.FindByUser(userId);
            
            return Result.Success(expenseItems);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<ExpenseItem>> UpdateItemPriority(Guid userId, Guid itemId, uint priority)
    {
        try
        {
            var expenseItem = await _expenseItemRepository.FindAsync(itemId);
            if (expenseItem is null) return Result.Failure<ExpenseItem>(Error.BadRequest("Item not found"));
            if (expenseItem.UserId != userId)
            {
                return Result.Failure<ExpenseItem>(Error.BadRequest("Item not found"));
            }
            
            var updatedItem  = await _expenseItemRepository.UpdatePriorityAsync(itemId, priority);
            if (updatedItem is null) return Result.Failure<ExpenseItem>(Error.BadRequest("Update failed"));

            return Result.Success(expenseItem);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<ExpenseItem>> UpdateItem(Guid userId)
    {
        try
        {
            await Task.Delay(1000);
            throw new NotImplementedException();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<string>> DeleteItem(Guid userId, Guid itemId)
    {
        try
        {
            var item = await _expenseItemRepository.FindAsync(itemId);
            if (item is null) return Result.Failure<string>(Error.BadRequest("Item not found"));
    
            if(item.UserId != userId) return Result.Failure<string>(Error.BadRequest("Item not found"));
            
            var deleted = await _expenseItemRepository.DeleteAsync(itemId);
            if (!deleted) return Result.Failure<string>(Error.BadRequest("Error deleting item"));
            
            return Result.Success("Item deleted successfully!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}