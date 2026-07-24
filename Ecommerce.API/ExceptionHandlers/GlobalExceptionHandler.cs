using Ecommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var logLevel = exception switch
            {
                BusinessRuleException => LogLevel.Warning,
                ConflictException => LogLevel.Warning,
                DomainValidationException => LogLevel.Warning,
                NotFoundException => LogLevel.Warning,
                UnauthorizedException => LogLevel.Warning,
                _ => LogLevel.Error
            };
            _logger.Log(logLevel, exception, exception.Message);

            var statusCode = exception switch
            {
                BusinessRuleException => StatusCodes.Status422UnprocessableEntity,
                ConflictException => StatusCodes.Status409Conflict,
                DomainValidationException => StatusCodes.Status400BadRequest,
                NotFoundException => StatusCodes.Status404NotFound,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            var problemDetails = new ProblemDetails()
            {
                Status = statusCode,
                Title = GetTitle(statusCode),
                Detail = GetDetails(exception),
                Type = $"https://httpstatuses.com/{statusCode}",
                Instance = httpContext.Request.Path
            };
            problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
            problemDetails.Extensions["timestamp"] = DateTime.UtcNow;
            
            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
        private static string GetTitle(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Not Found",
                409 => "Conflict",
                500 => "Internal Server Error",
                _ => "Error"
            };
        }
        private static string GetDetails(Exception exception)
        {
            return exception switch
            {
                NotFoundException ex => $"{ex.Resource} was not found.",
                ConflictException ex => ex.ClientMessage,
                DomainValidationException ex => ex.Message,
                BusinessRuleException ex => ex.Message,
                UnauthorizedException ex => ex.Message,
                InvalidOperationException ex => ex.Message,
                _ => "An unexpected error occurred."
            };
        }
    }
}

