using System.Security.Claims;

namespace WorkForYou.WebUI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal @this)
    {
        return @this.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    }
}
