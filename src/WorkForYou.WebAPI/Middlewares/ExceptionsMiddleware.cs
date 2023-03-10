using System.Net;
using WorkForYou.Core.AdditionalModels;

namespace WorkForYou.WebAPI.Middlewares;

public class ExceptionsMiddleware
{
    private readonly ILogger<ExceptionsMiddleware> _logger;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly RequestDelegate _next;

    public ExceptionsMiddleware(ILogger<ExceptionsMiddleware> logger, RequestDelegate next,
        IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _next = next;
        _hostEnvironment = hostEnvironment;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            string errorMessage;
            HttpStatusCode errorStatusCode;

            var errorType = ex.GetType();

            if (errorType == typeof(UnauthorizedAccessException))
            {
                errorStatusCode = HttpStatusCode.Unauthorized;
                errorMessage = "You are not authorized";
            }
            else
            {
                errorStatusCode = HttpStatusCode.InternalServerError;
                errorMessage = "Some unknown error occurred";
            }

            ApiError response = _hostEnvironment.IsDevelopment()
                ? new((int) errorStatusCode, ex.Message, ex.StackTrace)
                : new((int) errorStatusCode, errorMessage);

            _logger.LogError(ex, "Error: {Message}", ex.Message);

            context.Response.StatusCode = (int) errorStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(response.ToString());
        }
    }
}