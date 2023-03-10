namespace WorkForYou.WebUI.Middlewares;

public class PageErrorsMiddleware
{
    private readonly ILogger<PageErrorsMiddleware> _logger;
    private readonly RequestDelegate _next;

    public PageErrorsMiddleware(RequestDelegate next, ILogger<PageErrorsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response.StatusCode == 404)
            {
                const string pagePath = "/Error/PageNotFound";
                
                context.Request.Path = pagePath;
                await _next(context);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Page error");
        }
    }
}
