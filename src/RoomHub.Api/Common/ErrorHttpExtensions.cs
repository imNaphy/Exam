using Microsoft.AspNetCore.Http.HttpResults;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Common;

public static class ErrorHttpExtensions
{
    public static ProblemHttpResult ToProblem(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return TypedResults.Problem(
            detail: error.Message,
            statusCode: statusCode,
            title: error.Code);
    }
}
