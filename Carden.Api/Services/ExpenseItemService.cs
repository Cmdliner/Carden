using Carden.Api.Dtos;
using Carden.Api.Utils;

namespace Carden.Api.Services;

public interface IExpenseItemService
{
    public Task<Result<ExpenseItem>> AddItem(Guid user_id, AddExpenseItemDto expenseItemDto);
    public Task<Result<ExpenseItem>> GetItem(Guid item_id, Guid user_id);
    public Task<Result<List<ExpenseItem>>> GetExpenseItems(Guid user_id);
    public Task<Result<ExpenseItem>> UpdateItem(Guid item_id, Guid user_id);
}

public class ExpenseItemService(IExpenseItemRepository expenseItemRepository): IExpenseItemService
{

    private readonly IExpenseItemRepository _expenseItemRepository = expenseItemRepository;

    public async Task<Result<ExpenseItem>> AddItem(Guid user_id,  AddExpenseItemDto expenseItemDto)
    {

        var expenseItem = await _expenseItemRepository.CreateAsync(new ExpenseItem {
            UserId = user_id,
            Name = expenseItemDto.Name,
            Category = expenseItemDto.Category,
            Description = expenseItemDto.Description,
            ExpectedPrice = expenseItemDto.ExpectedPrice
        });

        return Result.Success<ExpenseItem>(expenseItem);
    }

    public async Task<Result<ExpenseItem>> GetItem(Guid item_id, Guid user_id)
    {
        try
        {
            var expenseItem = await _expenseItemRepository.FindAsync(item_id);
            if (expenseItem is null) return Result.Failure<ExpenseItem>(Error.BadRequest("expense item not found"));

            if (expenseItem.Id != user_id)
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

    public async Task<Result<List<ExpenseItem>>> GetExpenseItems(Guid user_id)
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

    public Task<Result<ExpenseItem>> UpdateItem(Guid item_id, Guid user_id)
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