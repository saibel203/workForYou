using Microsoft.AspNetCore.Http;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IFileService
{
    Task<FileResponse> UploadUserImageAsync(IFormFile userFile, UsernameDto? usernameDto);
}