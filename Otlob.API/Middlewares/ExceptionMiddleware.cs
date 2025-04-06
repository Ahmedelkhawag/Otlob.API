using Otlob.API.Errors;
using System.Net;
using System.Text.Json;

namespace Otlob.API.Middlewares
{
    public class ExceptionMiddleware
    {
        // To refer to the next component in the request processing pipeline.
        private readonly RequestDelegate _next;
        // To log the exception details.
        private readonly ILogger<ExceptionMiddleware> _logger;
        // To customize the error response based on the environment.
        private readonly IHostEnvironment _environment;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        // Handel server error
        public async Task InvokeAsync(HttpContext context)
        // HttpContext => contains all the information about the HTTP request and response
        {
            try
            {
                // If no error had occurred, request will passed to next middleware
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                // Log the exception details  
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Custom error response

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Enum

            var response = _environment.IsDevelopment()
                 // Development Environment
                 ? new ExceptionResponse(context.Response.StatusCode, ex.Message, ex.StackTrace.ToString())
                 // Production Environment
                 : new ExceptionResponse(context.Response.StatusCode);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var jsonResponse = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(jsonResponse);
        }

    }
}
