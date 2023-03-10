using AutoMapper;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.RepositoryInterfaces;
using WorkForYou.Infrastructure.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly WorkForYouDbContext _context;
    private bool _disposed;

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

    public UnitOfWork(WorkForYouDbContext context, ILoggerFactory loggerFactory, 
        IMapper mapper)
    {
        var logger = loggerFactory.CreateLogger("logs");
        
        _context = context;
        VacancyRepository = new VacancyRepository(context, logger, mapper);
        UserRepository = new UserRepository(context, logger);
        VacancyDomainRepository = new VacancyDomainRepository(context, logger);
        WorkCategoryRepository = new WorkCategoryRepository(context, logger);
        HowToWorkRepository = new HowToWorkRepository(context, logger);
        EnglishLevelRepository = new EnglishLevelRepository(context, logger);
        CandidateRegionRepository = new CandidateRegionRepository(context, logger);
        RelocateRepository = new RelocateRepository(context, logger);
        TypeOfCompanyRepository = new TypeOfCompanyRepository(context, logger);
        CommunicationLanguageRepository = new CommunicationLanguageRepository(context, logger);
        RespondedListRepository = new RespondedListRepository(context, logger);
        ChatRepository = new ChatRepository(context, logger);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
