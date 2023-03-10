namespace WorkForYou.Core.Models;

public class FavouriteCandidate
{
    public int EmployerUserId { get; set; }
    public EmployerUser? EmployerUser { get; set; }
    public int CandidateUserId { get; set; }
    public CandidateUser? CandidateUser { get; set; }
}
