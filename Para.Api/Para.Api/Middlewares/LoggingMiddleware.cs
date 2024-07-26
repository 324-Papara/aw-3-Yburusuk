using System.Text;

namespace Para.Api.Middlewares;

public class LoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<LoggingMiddleware> logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await LogRequest(context);

        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);

        await LogResponse(context);

        await responseBody.CopyToAsync(originalBodyStream);
    }

    private async Task LogRequest(HttpContext context)
    {
        context.Request.EnableBuffering();
        
        var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
        
        await context.Request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
        
        var requestBody = Encoding.UTF8.GetString(buffer);
        
        context.Request.Body.Position = 0;
        
        logger.LogInformation($"Request Body: {requestBody}");
    }

    private async Task LogResponse(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        
        logger.LogInformation($"Response Body: {responseBody}");
    }
}