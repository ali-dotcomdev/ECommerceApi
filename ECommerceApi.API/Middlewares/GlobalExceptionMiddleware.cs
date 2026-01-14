using System.Net;
using System.Text.Json;
using ECommerceApi.Application.Wrapper;

namespace ECommerceApi.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json"; // frontend
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        string message = "sunucu kaynakli bir hata olustu";

        switch (exception)
        {
            case KeyNotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case ArgumentException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                break;

            case ApplicationException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            default:
                message = exception.Message;
                break;
        }

        var response = new
        {
            Succeeded = false,
            Message = message,
            Errors = new[] {exception.Message}
        };

        var jsonResponse = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(jsonResponse);
    }
}
