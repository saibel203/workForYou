﻿namespace WorkForYou.Data.Models;

public class EnglishLevel
{
    public int EnglishLevelId { get; set; }
    public string NameLevel { get; set; } = string.Empty;
    public string DescriptionLevel { get; set; } = string.Empty;

    public CandidateUser? User { get; set; }
}
