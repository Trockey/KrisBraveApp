using DeveloperGoals.DTOs;
using DeveloperGoals.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace DeveloperGoals.Middleware;

/// <summary>
/// Globalny handler wyjątków dla aplikacji
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, error, message, details) = MapException(exception);

        // Logowanie w zależności od poziomu błędu
        LogException(exception, statusCode, context);

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(new ErrorResponseDto
        {
            Error = error,
            Message = message,
            Details = details
        }, cancellationToken);

        return true;
    }

    private (int StatusCode, string Error, string Message, string? Details) MapException(Exception exception)
    {
        return exception switch
        {
            // 400 Bad Request - Validation Errors
            ProfileIncompleteException ex => 
                (StatusCodes.Status400BadRequest, "ValidationError", ex.Message, null),
            
            TechnologyNotOwnedException ex => 
                (StatusCodes.Status400BadRequest, "ValidationError", ex.Message, null),
            
            InvalidTechnologyStatusException ex => 
                (StatusCodes.Status400BadRequest, "ValidationError", ex.Message, null),
            
            // 404 Not Found
            TechnologyNotFoundException ex => 
                (StatusCodes.Status404NotFound, "NotFound", ex.Message, null),
            
            // 408 Request Timeout
            AIServiceTimeoutException ex => 
                (StatusCodes.Status408RequestTimeout, "Timeout", ex.Message, null),
            
            // 500 Internal Server Error
            OpenRouterApiException ex => 
                (StatusCodes.Status500InternalServerError, "AIServiceError", ex.Message, ex.Details),
            
            AIResponseParsingException ex => 
                (StatusCodes.Status500InternalServerError, "AIServiceError", 
                 "Failed to parse AI response", ex.Message),
            
            // 502 Bad Gateway
            InvalidAIResponseFormatException ex => 
                (StatusCodes.Status502BadGateway, "BadGateway", ex.Message, null),
            
            // Default - 500 Internal Server Error
            _ => (StatusCodes.Status500InternalServerError, 
                  "InternalServerError", 
                  "An unexpected error occurred", 
                  null)
        };
    }

    private void LogException(Exception exception, int statusCode, HttpContext context)
    {
        var userId = context.User.Identity?.IsAuthenticated == true
            ? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            : "anonymous";

        var path = context.Request.Path;
        var method = context.Request.Method;

        switch (statusCode)
        {
            case >= 500:
                _logger.LogError(
                    exception,
                    "Server error {StatusCode} for {Method} {Path} by user {UserId}",
                    statusCode, method, path, userId);
                break;

            case >= 400:
                _logger.LogWarning(
                    "Client error {StatusCode} for {Method} {Path} by user {UserId}: {Message}",
                    statusCode, method, path, userId, exception.Message);
                break;

            default:
                _logger.LogInformation(
                    "Request completed with {StatusCode} for {Method} {Path} by user {UserId}",
                    statusCode, method, path, userId);
                break;
        }
    }
}
