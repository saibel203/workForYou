using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WorkForYou.WebUI.Attributes;

public class Unauthorized : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.User.Identity!.IsAuthenticated)
            context.Result = new RedirectResult("/Main/Index");
    }
}
