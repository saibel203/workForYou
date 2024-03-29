﻿namespace WorkForYou.Core.Models;

public class TypesOfCompany
{
    public int TypesOfCompanyId { get; set; }
    public string TypeOfCompanyName { get; set; } = string.Empty;
    public IEnumerable<Vacancy>? Vacancies { get; set; }
}
