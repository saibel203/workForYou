namespace WorkForYou.WebUI.Helpers;

public static class ViewHelpers
{
    public static string GetLastStringLetters(int number, bool isYear = false, bool isVacancy = false)
    {
        if (isYear)
        {
            if (number % 10 == 1)
                return "рік";
            if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
                return "років";
            return "роки";
        }

        if (isVacancy)
        {
            if (number % 10 == 1)
                return "вакансія";
            if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
                return "вакансій";
            return "вакансії";
        }

        if (number % 10 > 4 || number % 10 == 0 || number / 10 % 10 == 1)
            return "ів";
        if (number % 10 == 1)
            return "";
        return "и";
    }
}