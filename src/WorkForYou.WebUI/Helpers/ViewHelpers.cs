using System.Globalization;

namespace WorkForYou.WebUI.Helpers;

public static class ViewHelpers
{
    public static string GetLastStringLetters(int number,
        bool isYear = false, bool isVacancy = false)
    {
        var isEnCulture = CultureInfo.CurrentCulture.Equals(new CultureInfo("en"));
        
        if (isYear)
        {
            if (number % 10 == 1)
                return isEnCulture ? "year" : "рік";
            if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
                return isEnCulture ? "years" : "років";
            return isEnCulture ? "years" : "роки";
        }

        if (isVacancy)
        {
            if (number % 10 == 1)
                return isEnCulture ? "vacancy" : "вакансія";
            if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
                return isEnCulture ? "vacancies" : "вакансій";
            return isEnCulture ? "vacancies" : "вакансії";
        }

        if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
            return isEnCulture ? "s" : "ів";
        if (number % 10 == 1)
            return "";
        return isEnCulture ? "s" : "и";
    }
}