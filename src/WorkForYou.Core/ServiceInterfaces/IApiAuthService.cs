using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IApiAuthService
{
    Task<UserAuthResponse> GetUserAccessTokenAsync(UserLoginDto? userLoginDto);

}
