using Newtonsoft.Json;
using System.Net;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized, "Unauthorized");
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest, "Bad Request");
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound, "Not Found");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode, string error)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonConvert.SerializeObject(new
            {
                status = (int)statusCode,
                error = error,
                message = exception.Message
            });

            return context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}