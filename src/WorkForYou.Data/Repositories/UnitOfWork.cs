using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Data.DatabaseContext;

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
    public ITypeOfCompanyRepository TypeOfCompanyRepository { get; set; }

    public UnitOfWork(WorkForYouDbContext context, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor, 
        UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        var logger = loggerFactory.CreateLogger("logs");
        
        _context = context;
        VacancyRepository = new VacancyRepository(context, logger, httpContextAccessor, userManager, mapper);
        UserRepository = new UserRepository(context, logger, httpContextAccessor, userManager, mapper);
        VacancyDomainRepository = new VacancyDomainRepository(context, logger, httpContextAccessor, userManager, mapper);
        WorkCategoryRepository = new WorkCategoryRepository(context, logger, httpContextAccessor, userManager, mapper);
        HowToWorkRepository = new HowToWorkRepository(context, logger, httpContextAccessor, userManager, mapper);
        EnglishLevelRepository = new EnglishLevelRepository(context, logger, httpContextAccessor, userManager, mapper);
        CandidateRegionRepository = new CandidateRegionRepository(context, logger, httpContextAccessor, userManager, mapper);
        RelocateRepository = new RelocateRepository(context, logger, httpContextAccessor, userManager, mapper);
        TypeOfCompanyRepository = new TypeOfCompanyRepository(context, logger, httpContextAccessor, userManager, mapper);
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
