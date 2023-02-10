using Microsoft.AspNetCore.Http;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.IServices;

public interface IFileService
{
    Task<FileResponse> UploadUserImageAsync(IFormFile userFile);
    FileResponse RemoveUserImageAsync(string filePath);
}