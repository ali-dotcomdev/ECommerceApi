using ECommerceApi.Application.Wrapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace ECommerceApi.API.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    private readonly static JsonSerializerOptions _jsonSerializer = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
      

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

        var responseModel = new Result<string>
        { 
            Succeeded = false, 
            Message = "sunuc kaynakli beklenmeyen bir hata olustu." 
        };
        _logger.LogError(exception, "Bir hata yakalandi: {Message}", exception.Message);

        switch (exception)
        {
            case ValidationException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Message = "validasyon hatasi olustu";
                responseModel.Errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                break;
            case KeyNotFoundException ex:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                responseModel.Message = ex.Message;
                responseModel.Errors = new List<string> { ex.Message };
                break;

            case UnauthorizedAccessException ex:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                responseModel.Message = ex.Message;
                responseModel.Errors = new List<string> { ex.Message };
                break;

            case ArgumentException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Message = ex.Message;
                responseModel.Errors = new List<string> { ex.Message };
                break;

            case ApplicationException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Message = ex.Message;
                responseModel.Errors = new List<string> { ex.Message };
                break;
            case InvalidOperationException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                responseModel.Message = ex.Message;
                responseModel.Errors = new List<string> { ex.Message };
                break;
            case DbUpdateException ex:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                responseModel.Message = ex.Message;
                responseModel.Errors = new List<string> { ex.Message };
                break;
            default:
                _logger.LogError(exception, "bilinmeyen hata:{Message}", exception.Message);
                responseModel.Message = "beklenmeye bir hata olustu";
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(responseModel, _jsonSerializer);

        return context.Response.WriteAsync(jsonResponse);
    }
}
