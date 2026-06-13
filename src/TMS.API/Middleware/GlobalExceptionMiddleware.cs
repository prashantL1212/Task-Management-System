using TMS.Shared.Models;

namespace TMS.API.Middleware;

/// <summary>
/// Catches unhandled exceptions, logs them, and returns a standardized
/// <see cref="ApiResponse{T}"/> error with HTTP 500.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing {Method} {Path}",
                context.Request.Method, context.Request.Path);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = ApiResponse.Failure<object>("An unexpected error occurred.");
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
