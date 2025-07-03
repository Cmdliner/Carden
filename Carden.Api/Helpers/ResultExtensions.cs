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
        if (result.IsSuccess)
        {
            var response = new ApiResponse<T>
            {
                Success = true,
                Data = result.Value,
                Message = successMessage
            };

            return new ObjectResult(response) { StatusCode = successStatusCode };
        }

        return result.Error.ToActionResult();
    }

    public static IActionResult ToActionResult(this Result result, int successStatusCode, string successMessage)
    {
        if (result.IsSuccess)
        {
            var response = new ApiResponse
            {
                Success = true,
                Message = successMessage
            };

            return new ObjectResult(response) { StatusCode = successStatusCode };
        }

        return result.Error.ToActionResult();
    }

    // Convenience methods for common scenarios
    public static IActionResult ToCreatedResult<T>(this Result<T> result, string? location = null)
    {
        if (result.IsSuccess)
        {
            var response = new ApiResponse<T>
            {
                Success = true,
                Data = result.Value,
                Message = "Resource created successfully"
            };

            var createdResult = new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
            
            if (!string.IsNullOrEmpty(location))
            {
                // Add Location header for RESTful compliance
                createdResult.Value = response;
                return new CreatedResult(location, response);
            }

            return createdResult;
        }

        return result.Error.ToActionResult();
    }

    public static IActionResult ToCreatedResult(this Result result, string? location = null)
    {
        if (result.IsSuccess)
        {
            var response = new ApiResponse
            {
                Success = true,
                Message = "Resource created successfully"
            };

            if (!string.IsNullOrEmpty(location))
            {
                return new CreatedResult(location, response);
            }

            return new ObjectResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        return result.Error.ToActionResult();
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
        if (result.IsSuccess)
        {
            return new NoContentResult();
        }

        return result.Error.ToActionResult();
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
    
    
    /*
     public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new ApiResponse<T>
            {
                Success = true,
                Data = result.Value,
                Message = "Request completed successfully"
            });
        }

        return result.Error.ToActionResult();
    }

    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(new ApiResponse
            {
                Success = true,
                Message = "Request completed successfully"
            });
        }

        return result.Error.ToActionResult();
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
    */
}