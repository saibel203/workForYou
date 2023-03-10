using Microsoft.AspNetCore.Identity;

namespace WorkForYou.Core.Responses.Services;

public class UserAuthResponse : BaseResponse
{
    public IEnumerable<IdentityError>? Errors { get; set; }
    public bool IsUserCandidate { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
