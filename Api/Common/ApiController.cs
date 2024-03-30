using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

[ApiController]
public class ApiController : ControllerBase
{
    protected IResult Problem(IEnumerable<IError> errors)
    {
        return Results.Problem(
            detail: errors.First().Message,
            statusCode: StatusCodes.Status400BadRequest
        );
    }
}
