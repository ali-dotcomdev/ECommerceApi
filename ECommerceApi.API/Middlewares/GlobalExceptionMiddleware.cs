using System.Net;
using System.Text.Json;
using ECommerceApi.Application.Wrapper;

namespace ECommerceApi.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json"; // frontend
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var responseModel = new { Succeeded = false, Message = "sunuc kaynakli bilinmeyen bir hata olustu." };
        _logger.LogError(exception, "Bir hata yakalandi: {Message}", exception.Message);

        switch (exception)
        {
            case KeyNotFoundException ex:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                responseModel = new {Succeeded = false, Message = ex.Message};
                break;

            case UnauthorizedAccessException ex:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                responseModel = new { Succeeded = false, Message = ex.Message };
                break;

            case ArgumentException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel = new { Succeeded = false, Message = ex.Message };
                break;

            case ApplicationException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel = new { Succeeded = false, Message = ex.Message };
                break;

            default:
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; //frontend

        var jsonResponse = JsonSerializer.Serialize(responseModel, options);

        return context.Response.WriteAsync(jsonResponse);
    }
}
