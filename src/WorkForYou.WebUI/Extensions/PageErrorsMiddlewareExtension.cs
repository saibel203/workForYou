using WorkForYou.WebUI.Middlewares;

namespace WorkForYou.WebUI.Extensions;

public static class PageErrorsMiddlewareExtension
{
    public static void UsePageErrors(this IApplicationBuilder app)
    {
        app.UseMiddleware<PageErrorsMiddleware>();
    }
}
