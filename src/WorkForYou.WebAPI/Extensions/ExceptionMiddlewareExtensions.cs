using WorkForYou.WebAPI.Middlewares;

namespace WorkForYou.WebAPI.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseExceptionMiddlewareHandler(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ExceptionsMiddleware>();
    }
}
