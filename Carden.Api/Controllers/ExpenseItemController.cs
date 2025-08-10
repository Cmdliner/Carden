using Asp.Versioning;
using Carden.Api.Dtos;
using Carden.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Controllers;

[ApiController]
[ApiVersion(1)]
[Authorize]
[Route("api/v{v:apiVersion}/expense-items")]
public class ExpenseItemController(IExpenseItemService expenseItemService) : ControllerBase
{
    private readonly IExpenseItemService _expenseItemService = expenseItemService;

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExpenseItem(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService.GetItem(id, Guid.Parse(userId));
        return result.ToActionResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetExpenseItems()
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService.GetItems(Guid.Parse(userId));
        return result.ToActionResult();
    }

    [HttpPost]
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

    [HttpPut("{id:guid}/priority")]
    public async Task<IActionResult> UpdateItemPriority(
        [FromBody] UpdateItemPriorityDto updateItemPriorityDto,
        Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService
            .UpdateItemPriority(Guid.Parse(userId), id, updateItemPriorityDto.NewPriority);

        return result.ToActionResult();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExpenseItem(Guid id)
    {
        var userId = User.FindFirst("sub")?.Value;
        if (userId is null)
        {
            var errorDetails = new ErrorDetails("403", "Unauthorized");
            var apiResponse = new ApiResponse(false, "Unauthorized", errorDetails);
            return Unauthorized(apiResponse);
        }

        var result = await _expenseItemService.DeleteItem(Guid.Parse(userId), id);
        return result.ToActionResult();
    }
}