using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.ServiceInterfaces;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<FileService> _logger;
    private readonly IUnitOfWork _unitOfWork; 
    
    public FileService(IWebHostEnvironment webHostEnvironment, ILogger<FileService> logger, IUnitOfWork unitOfWork)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<FileResponse> UploadUserImageAsync(IFormFile userFile, UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };
            
            var webRootPath = _webHostEnvironment.WebRootPath;
            var uploadDirectory = Path.Combine(webRootPath, "img", "userImages");
            var fileExtension = Path.GetExtension(userFile.FileName);
            var fileName = ImageNameGeneration();
            var resultPath = Path.Combine("img", "userImages", fileName + fileExtension);

            if (userFile.Length > 0)
            {
                IsDirectoryExists(uploadDirectory);
                
                await using var fileStream = new FileStream(Path.Combine(uploadDirectory, fileName + fileExtension), FileMode.Create);
                await userFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                var userData = await _unitOfWork.UserRepository.GetUserDataAsync(usernameDto);
                
                if (string.IsNullOrEmpty(userData.User.ImagePath))
                {
                    userData.User.ImagePath = $"\\{resultPath}";
                    await _unitOfWork.SaveAsync();
                }
                else
                {
                    var removeOldResult = RemoveUserImageAsync(userData.User.ImagePath);

                    if (!removeOldResult.IsSuccessfully)
                        return new()
                        {
                            Message = "An error occurred while trying to delete an old user photo",
                            IsSuccessfully = false
                        };

                    userData.User.ImagePath = $"\\{resultPath}";
                    await _unitOfWork.SaveAsync();
                }
                    
                return new()
                {
                    Message = "Image uploaded successfully",
                    IsSuccessfully = true,
                    FilePath = @"\" + resultPath
                };
            }

            return new()
            {
                Message = "Error uploading image",
                IsSuccessfully = false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return new()
            {
                Message = "Error uploading image",
                IsSuccessfully = false
            };
        }
    }

    private FileResponse RemoveUserImageAsync(string filePath)
    {
        try
        {
            var rootPath = _webHostEnvironment.WebRootPath;
            var fullPath = rootPath + filePath;
            
            if (!File.Exists(fullPath))
                return new()
                {
                    Message = "Error when deleting pictures",
                    IsSuccessfully = false
                };
            
            File.Delete(fullPath);

            return new()
            {
                Message = "The image has been successfully deleted",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error when deleting pictures");
            return new()
            {
                Message = "Error when deleting pictures",
                IsSuccessfully = false
            };
        }
        
    }

    private void IsDirectoryExists(string uploadDirectory)
    {
        if (!Directory.Exists(uploadDirectory))
            Directory.CreateDirectory(uploadDirectory);
    }

    private string ImageNameGeneration()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, 30)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }
}
