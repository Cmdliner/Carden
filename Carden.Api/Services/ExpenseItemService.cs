using Carden.Api.Dtos;
using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IExpenseItemService
{
    public Task<Result<ExpenseItem>> AddItem(Guid user_id, AddExpenseItemDto expenseItemDto);
    public Task<Result<ExpenseItem>> GetItem(Guid item_id, Guid user_id);
    public Task<Result<List<ExpenseItem>>> GetItems(Guid user_id);
    public Task<Result<ExpenseItem>> UpdateItemPriority(Guid user_id, Guid item_id, uint priority);
    public Task<Result<ExpenseItem>> UpdateItem(Guid user_id);

}

public class ExpenseItemService(IExpenseItemRepository expenseItemRepository): IExpenseItemService
{

    private readonly IExpenseItemRepository _expenseItemRepository = expenseItemRepository;

    public async Task<Result<ExpenseItem>> AddItem(Guid user_id,  AddExpenseItemDto expenseItemDto)
    {
        var expenseItem = new ExpenseItem
        {
            UserId = user_id,
            Name = expenseItemDto.Name,
            Category = expenseItemDto.Category,
            Description = expenseItemDto.Description,
            ExpectedPrice = expenseItemDto.ExpectedPrice
        };
        
        var expenseItemInDb = await _expenseItemRepository.AddItemAsync(expenseItem, expenseItemDto.Priority);

        return Result.Success<ExpenseItem>(expenseItemInDb);
    }

    public async Task<Result<ExpenseItem>> GetItem(Guid item_id, Guid user_id)
    {
        try
        {
            var expenseItem = await _expenseItemRepository.FindAsync(item_id);
            if (expenseItem is null) return Result.Failure<ExpenseItem>(Error.BadRequest("expense item not found"));

            if (expenseItem.UserId != user_id)
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

    public async Task<Result<List<ExpenseItem>>> GetItems(Guid user_id)
    {
        try
        {
            var expenseItems = await _expenseItemRepository.FindByUser(user_id);
            
            return Result.Success<List<ExpenseItem>>(expenseItems);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<ExpenseItem>> UpdateItemPriority(Guid user_id, Guid item_id, uint priority)
    {
        try
        {
            var expenseItem = await _expenseItemRepository.FindAsync(item_id);
            if (expenseItem.UserId != user_id)
            {
                return Result.Failure<ExpenseItem>(Error.BadRequest("Item not found"));
            }
            var updatedItem  = await _expenseItemRepository.UpdatePriorityAsync(item_id, priority);
            if (updatedItem is null)
            {
                return Result.Failure<ExpenseItem>(Error.BadRequest("Update failed"));
            }

            return Result.Success<ExpenseItem>(expenseItem);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<ExpenseItem>> UpdateItem(Guid user_id)
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
}