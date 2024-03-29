﻿namespace WorkForYou.Core.RepositoryInterfaces;

public interface IUnitOfWork : IDisposable
{
    public IVacancyRepository VacancyRepository { get; }
    public IUserRepository UserRepository { get; }
    public IVacancyDomainRepository VacancyDomainRepository { get; }
    public IWorkCategoryRepository WorkCategoryRepository { get; }
    public IHowToWorkRepository HowToWorkRepository { get; }
    public IEnglishLevelRepository EnglishLevelRepository { get; }
    public ICandidateRegionRepository CandidateRegionRepository { get; }
    public IRelocateRepository RelocateRepository { get; }
    public ITypeOfCompanyRepository TypeOfCompanyRepository { get; }
    public ICommunicationLanguageRepository CommunicationLanguageRepository { get; }
    public IRespondedListRepository RespondedListRepository { get; }
    public IChatRepository ChatRepository { get; }
    Task SaveAsync();
}