using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Carden.Api.Utils;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occured");
        var response = new ApiResponse
        {
            Success = false,
            Error = new ErrorDetails
            {
                Code = "INTERNAL_ERROR",
                Message = "An internal server error occurred"
            }
        };
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/json";

        await httpContext.Response.WriteAsync(
            JsonSerializer.Serialize(response), 
            cancellationToken);

        return true;
    }
    
    
}