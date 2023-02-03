using Microsoft.AspNetCore.Identity;

namespace WorkForYou.Core.Responses.Services;

public class UserAuthResponse : BaseResponse
{
    public IEnumerable<IdentityError>? Errors { get; set; }
}
