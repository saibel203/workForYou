using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkForYou.Core.IRepositories;
using WorkForYou.Core.Models.IdentityInheritance;
using WorkForYou.Data.DatabaseContext;

namespace WorkForYou.Data.Repositories;

public class GenericRepository<T>: IGenericRepository<T> where T: class
{
    protected WorkForYouDbContext Context;
    protected readonly ILogger Logger;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected readonly UserManager<ApplicationUser> UserManager;
    protected readonly IMapper Mapper;
    internal DbSet<T> DbSet;

    public GenericRepository(WorkForYouDbContext context, ILogger logger, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        Context = context;
        Logger = logger;
        HttpContextAccessor = httpContextAccessor;
        UserManager = userManager;
        Mapper = mapper;
        DbSet = Context.Set<T>();
    }

    public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, 
        IOrderedQueryable<T>>? orderBy, string includeProperties = "")
    {
        IQueryable<T> query = DbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split
                     (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        
        return await query.ToListAsync();
        
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }
    
    public async Task RemoveAsync(object id)
    {
        var entity = await DbSet.FindAsync(id);
        Remove(entity!);
    }
    
    public void Remove(T entity)
    {
        if (Context.Entry(entity).State == EntityState.Detached)
            DbSet.Attach(entity);

        DbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        DbSet.Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }
}
