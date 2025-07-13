using Microsoft.AspNetCore.Mvc;

namespace Carden.Api.Helpers;
public static class ResultExtensions
{
   public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        return result.ToActionResult(StatusCodes.Status200OK, "Request completed successfully");
    }

    public static IActionResult ToActionResult(this Result result)
    {
        return result.ToActionResult(StatusCodes.Status200OK, "Request completed successfully");
    }

    // Overloads for custom success status codes
    public static IActionResult ToActionResult<T>(this Result<T> result, int successStatusCode, string successMessage)
    {
        if (!result.IsSuccess) return result.Error.ToActionResult();
        var response = new ApiResponse<T>
        {
            Success = true,
            Data = result.Value,
            Message = successMessage
        };

        return new ObjectResult(response) { StatusCode = successStatusCode };

    }

    public static IActionResult ToActionResult(this Result result, int successStatusCode, string successMessage)
    {
        if (!result.IsSuccess) return result.Error.ToActionResult();
        var response = new ApiResponse
        {
            Success = true,
            Message = successMessage
        };

        return new ObjectResult(response) { StatusCode = successStatusCode };

    }
    public static IActionResult ToCreatedResult<T>(this Result<T> result, string? location = null)
    {
        if (!result.IsSuccess) return result.Error.ToActionResult();
        var response = new ApiResponse<T>
        {
            Success = true,
            Data = result.Value,
            Message = "Resource created successfully"
        };

        var createdResult = new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };

        if (string.IsNullOrEmpty(location)) return createdResult;
        
        createdResult.Value = response;
        return new CreatedResult(location, response);

    }

    public static IActionResult ToCreatedResult(this Result result, string? location = null)
    {
        if (!result.IsSuccess) return result.Error.ToActionResult();
        var response = new ApiResponse
        {
            Success = true,
            Message = "Resource created successfully"
        };

        return !string.IsNullOrEmpty(location) ? new CreatedResult(location, response) : new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };

    }

    public static IActionResult ToAcceptedResult<T>(this Result<T> result)
    {
        return result.ToActionResult(StatusCodes.Status202Accepted, "Request accepted for processing");
    }

    public static IActionResult ToAcceptedResult(this Result result)
    {
        return result.ToActionResult(StatusCodes.Status202Accepted, "Request accepted for processing");
    }

    public static IActionResult ToNoContentResult(this Result result)
    {
        return result.IsSuccess ? new NoContentResult() : result.Error.ToActionResult();
    }

    public static IActionResult ToActionResult(this Error error)
    {
        var response = new ApiResponse
        {
            Success = false,
            Error = new ErrorDetails
            {
                Code = error.Code,
                Message = error.Message
            }
        };

        return error.Type switch
        {
            ErrorType.BadRequest => new BadRequestObjectResult(response),
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(response),
            ErrorType.Forbidden => new ObjectResult(response) { StatusCode = 403 },
            ErrorType.Conflict => new ConflictObjectResult(response),
            _ => new ObjectResult(response) { StatusCode = 500 }
        };
    }
}