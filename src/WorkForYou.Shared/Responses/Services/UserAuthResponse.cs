using Microsoft.AspNetCore.Identity;

namespace WorkForYou.Shared.Responses.Services;

public class UserAuthResponse : BaseResponse
{
    public IEnumerable<IdentityError>? Errors { get; set; }
}
