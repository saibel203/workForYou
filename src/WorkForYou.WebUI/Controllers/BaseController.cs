using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WorkForYou.WebUI.Controllers;

public class BaseController : Controller
{
    public string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
    }

    public static string GetLastStringLetters(int number, bool isYear = false)
    {
        if (isYear)
        {
            if (number % 10 == 1)
                return " рік";
            if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
                return "років";
            return " роки";
        }

        if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
            return "ів";
        if (number % 10 == 1)
            return "";
        return "и";
    }
}