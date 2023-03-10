using Microsoft.Extensions.Logging;
using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Core.Responses.Services;
using WorkForYou.Core.ServiceInterfaces;

namespace WorkForYou.Services;

public class ViewCounterService : IViewCounterService
{
    private readonly ILogger<ViewCounterService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ViewCounterService(ILogger<ViewCounterService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ViewCounterResponse> UpdateCandidateViewNumberCountAsync(UsernameDto? usernameDto)
    {
        try
        {
            if (usernameDto is null)
                return new()
                {
                    Message = "",
                    IsSuccessfully = false
                };
            
            var userResult = await _unitOfWork.UserRepository.GetUserDataAsync(usernameDto);
    
            if (!userResult.IsSuccessfully || userResult.User.CandidateUser is null)
                return new()
                {
                    Message = "Error getting user",
                    IsSuccessfully = false
                };
    
            userResult.User.CandidateUser.ViewCount++;
            await _unitOfWork.SaveAsync();
    
            return new()
            {
                Message = "Account views counter updated successfully",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "");
            return new()
            {
                Message = "",
                IsSuccessfully = false
            };
        }
    }
    
    public async Task<ViewCounterResponse> UpdateViewNumberOfCountAsync(int vacancyId)
    {
        try
        {
            var vacancyResult = await _unitOfWork.VacancyRepository
                .GetVacancyByIdAsync(vacancyId);

            if (!vacancyResult.IsSuccessfully || vacancyResult.Vacancy is null)
                return new()
                {
                    Message = "An error occurred when receiving a vacancy",
                    IsSuccessfully = false
                };

            vacancyResult.Vacancy.ViewCount++;
            await _unitOfWork.SaveAsync();

            return new()
            {
                Message = "Job view counter has been updated successfully",
                IsSuccessfully = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while trying to update the job view counter");
            return new()
            {
                Message = "An error occurred while trying to update the job view counter",
                IsSuccessfully = false
            };
        }
    }
}
