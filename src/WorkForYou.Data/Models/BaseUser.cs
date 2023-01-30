using WorkForYou.Data.Models.IdentityInheritance;

namespace WorkForYou.Data.Models;

public class BaseUser
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
}
