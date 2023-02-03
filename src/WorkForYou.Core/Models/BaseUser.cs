using WorkForYou.Core.Models.IdentityInheritance;

namespace WorkForYou.Core.Models;

public class BaseUser
{
    public string ApplicationUserId { get; set; } = string.Empty;
    public ApplicationUser? ApplicationUser { get; set; }
}
