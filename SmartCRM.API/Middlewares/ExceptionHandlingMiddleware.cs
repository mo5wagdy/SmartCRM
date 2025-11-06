using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmartCRM.Application.Exceptions;

namespace SmartCRM.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context,Exception exception)
        {
            context.Response.ContentType = "application/problem+json";

            var status = HttpStatusCode.InternalServerError;
            var Title = "An unexpected error occurred.";
            var detail = exception.Message;
            var traceId = context.TraceIdentifier;

            switch (exception)
            {
                case NotFoundException:
                    status = HttpStatusCode.NotFound;
                    Title = "Not Found";
                    break;
                case BusinessRuleException:
                    status = HttpStatusCode.BadRequest;
                    Title = "Business Rule Violation";
                    break;
                case FluentValidation.ValidationException fv:
                    status = HttpStatusCode.BadRequest;
                    Title = "Validation Failed";
                    detail = "See Errors Property For Details";
                    break;
                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    Title = "Unauthorized Access";
                    break;
            }

            // log server errors
            if ((int)status >= 500)
            {
                _logger.LogError(exception, "Unhandled exception: {TraceId}", traceId);
            }
            else
            {
                _logger.LogWarning(exception, "Handled exception: {Type} - {Message}", exception.GetType().Name, exception.Message);
            }

            var problem = new
            {
                type = $"https://httpstatuses.io/{(int)status}",
                status = (int)status,
                detail,
                instance = context.Request.Path,
                traceId
            };

            context.Response.StatusCode = (int)status;
            var json = JsonSerializer.Serialize(problem);
            return context.Response.WriteAsync(json);
        }
    }
}
