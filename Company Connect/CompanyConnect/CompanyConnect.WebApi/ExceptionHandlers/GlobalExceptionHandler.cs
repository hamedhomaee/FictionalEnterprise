using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace CompanyConnect.WebApi.ExceptionHandlers;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService
) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    private readonly IProblemDetailsService _problemDetailsService = problemDetailsService;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException =>
                (StatusCodes.Status400BadRequest, "Validation failed"),

            KeyNotFoundException =>
                (StatusCodes.Status404NotFound, "Resource not found"),

            UnauthorizedAccessException =>
                (StatusCodes.Status403Forbidden, "Forbidden"),

            InvalidOperationException =>
                (StatusCodes.Status409Conflict, "Conflict"),

            ArgumentException =>
                (StatusCodes.Status400BadRequest, "Invalid request"),

            _ =>
                (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        if (statusCode >= 500)
        {
            _logger.LogError(
                exception,
                "A server-side error occurred while processing the request.");
        }
        else
        {
            _logger.LogWarning(
                exception,
                "A client-side error occurred while processing the request.");
        }

        httpContext.Response.StatusCode = statusCode;

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails =
                {
                    Status = statusCode,
                    Title = title,
                    Detail = statusCode == 500
                        ? "An unexpected server error occurred."
                        : exception.Message,
                    Instance = httpContext.Request.Path
                }
            });
    }
}