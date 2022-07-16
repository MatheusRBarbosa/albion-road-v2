using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AlbionRoad.Domain.Exceptions;

namespace AlbionRoad.API.Exceptions;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ExceptionFilter()
    {
        // Exceptions esperadas e seu respectivo tratamento.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(InvalidOperationException), HandleNotFoundException },
                { typeof(KeyNotFoundException), HandleNotFoundException },
                { typeof(InvalidRouteException), HandleBadRequestException },
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var errorResponse = new ErrorResponseWithException(context.Exception.Message, StatusCodes.Status500InternalServerError, "UnHandleException");

        context.Result = ErrorToObjectResult(errorResponse, StatusCodes.Status500InternalServerError);

        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var errorResponse = new ErrorResponseWithException(context.Exception.Message, StatusCodes.Status404NotFound, "NotFoundException");
        context.Result = ErrorToObjectResult(errorResponse, StatusCodes.Status404NotFound);
        context.ExceptionHandled = true;
    }

    private void HandleBadRequestException(ExceptionContext context)
    {
        var errorResponse = new ErrorResponseWithException(context.Exception.Message, StatusCodes.Status400BadRequest, "BadRequestException");
        context.Result = ErrorToObjectResult(errorResponse, StatusCodes.Status400BadRequest);
        context.ExceptionHandled = true;
    }

    private ObjectResult ErrorToObjectResult(Object error, int statusCode)
    {
        return new ObjectResult(error)
        {
            StatusCode = statusCode
        };
    }
}
