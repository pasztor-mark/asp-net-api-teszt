using System.Text.Json;

namespace api_teszt.Helpers;

public class GlobalExceptionHandler
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    
    public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            await HandleExceptionAsync(ctx, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext ctx, Exception e)
    {
        var res = new
        {
            message = "An unexpected error occured",
            error = e.Message,
        };

        var body = JsonSerializer.Serialize(res);
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = 500;
        
        return ctx.Response.WriteAsync(body);
    }
}