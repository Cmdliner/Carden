using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/expense-items")]
public class ExpenseItemController(IExpenseItemService expenseItemService) : ControllerBase
{
    private readonly IExpenseItemService _expenseItemService = expenseItemService;
    
    [HttpGet()]
    public async Task<IActionResult> GetExpenseItems()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService.GetExpenseItems(Guid.Parse(userId));
        return result.ToActionResult();
    }
    
    [HttpPost()]
    public async Task<IActionResult> AddExpenseItem([FromBody] AddExpenseItemDto addExpenseItemDto)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService.AddItem(Guid.Parse(userId), addExpenseItemDto);
        return result.ToCreatedResult();
    }

    [HttpGet("{item_id:guid}")]
    public async  Task<IActionResult> GetExpenseItem(Guid item_id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService.GetItem(item_id, Guid.Parse(userId));
        return result.ToActionResult();
    }
}