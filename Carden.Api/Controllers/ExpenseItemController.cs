using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Carden.Api.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class ExpenseItemController(IExpenseItemService expenseItemService) : ControllerBase
{
    private readonly IExpenseItemService _expenseItemService = expenseItemService;

    [HttpPost("add")]
    public async Task<IActionResult> AddExpenseItem(AddExpenseItemDto dto)
    {
        try
        {
            var userIdClaim = User.FindFirstValue("sub");
            if (userIdClaim is null) return Unauthorized(new { Success = false, Message = "unauthorized" });

            var userId = Guid.Parse(userIdClaim);
            await _expenseItemService.AddExpenseItem(dto, userId);
            
            return Created(nameof(AddExpenseItem), new { Success = true, Message = "Item added successfully" });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }

    [HttpGet("")]
    public async Task<IActionResult> GetExpenseItems()
    {
        try
        {
            var userIdClaim = User.FindFirstValue("sub");
            if (userIdClaim is null) return Unauthorized(new { Success = false, Message = "unauthorized" });

            var userId = Guid.Parse(userIdClaim);
            var expenseItems = await _expenseItemService.GetExpenseItems(userId);

            return Ok(new { Success = true, Message = "Items found", Data = new { ExpenseItems = expenseItems } });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest((new { Success = false, Message = e.Message }));
        }
    }

    [HttpPost("/{expenseItemId:guid}/update-priority")]
    public async Task<IActionResult> UpdateExpenseItemPriority([FromBody] double priority, Guid expenseItemId)
    {
        try
        {
            var userIdClaim = User.FindFirstValue("sub");
            if (userIdClaim is null) return Unauthorized(new { Success = false, Message = "unauthorized" });

            var userId = Guid.Parse(userIdClaim);
            await _expenseItemService.UpdatePriority(expenseItemId, userId, priority);

            return Ok(new { Success = true, Message = "Item priority updated" });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }

    [HttpDelete("/{expenseItemId:guid}/remove")]
    public async Task<IActionResult> RemoveExpenseItem(Guid expenseItemId)
    {
        try
        {
            var userIdClaim = User.FindFirstValue("sub");
            if (userIdClaim is null) return Unauthorized(new { Success = false, Message = "unauthorized" });

            var userId = Guid.Parse(userIdClaim);
            await _expenseItemService.RemoveExpenseItem(expenseItemId, userId);
            return Ok(new { Success = true, Message = "Item deleted successfully" });
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(new { Success = false, Message = e.Message });
        }
    }
}

public record AddExpenseItemDto(string Name, long Amount, double? Priority = null, string? Description = null);