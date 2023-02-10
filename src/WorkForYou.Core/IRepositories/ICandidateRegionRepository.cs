﻿using WorkForYou.Core.Responses.Repositories;

namespace WorkForYou.Core.IRepositories;

public interface ICandidateRegionRepository
{
    Task<CandidateRegionResponse> GetAllCandidateRegionsAsync();
}