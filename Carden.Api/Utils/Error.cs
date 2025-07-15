namespace Carden.Api.Utils;

public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    
    public static Error NotFound(string message) => new("NOT_FOUND", message, ErrorType.NotFound);
    public static Error BadRequest(string message) => new("BAD_REQUEST", message, ErrorType.BadRequest);
    public static Error Unauthorized(string message) => new("UNAUTHORIZED", message, ErrorType.Unauthorized);
    public static Error Forbidden(string message) => new("FORBIDDEN", message, ErrorType.Forbidden);
    public static Error Conflict(string message) => new("CONFLICT", message, ErrorType.Conflict);
    public static Error Validation(string message) => new("VALIDATION_ERROR", message, ErrorType.BadRequest);
    public static Error Internal(string message) => new("INTERNAL_ERROR", message, ErrorType.Failure);
    
    public static implicit operator string(Error error) => error.Code;
}

public enum ErrorType
{
    Failure = 0,
    BadRequest = 1,
    NotFound = 2,
    Unauthorized = 3,
    Forbidden = 4,
    Conflict = 5
}