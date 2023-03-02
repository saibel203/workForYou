﻿using WorkForYou.Core.DTOModels.UserDTOs;
using WorkForYou.Core.Responses.Services;

namespace WorkForYou.Core.ServiceInterfaces;

public interface IVacancyService
{
    Task<VacancyResponse> AddVacancyToFavouriteListAsync(UsernameDto? usernameDto, int vacancyId);
    Task<VacancyResponse> IsVacancyInFavouriteListAsync(int vacancyId, int candidateId);
}