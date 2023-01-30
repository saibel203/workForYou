namespace WorkForYou.Data.Models;

public class WorkCategory
{
    public int WorkCategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;

    public CandidateUser? User { get; set; }
}
