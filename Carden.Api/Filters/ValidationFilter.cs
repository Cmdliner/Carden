using Carden.Api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Carden.Api.Filters;

public class ValidationFilter(IWebHostEnvironment env) : IActionFilter
{
    private readonly IWebHostEnvironment _env = env;
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            var errorMessage = string.Join("; ", errors);
            context.Result = new BadRequestObjectResult(new ApiResponse
            {
                Error = new ErrorDetails { Code = "400", Message = errorMessage },
                Success = false,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) {}
}