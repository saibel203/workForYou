using System.Linq.Expressions;

namespace WorkForYou.Core.IRepositories;

public interface IGenericRepository<T> where T: class
{
    Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter, Func<IQueryable<T>,
        IOrderedQueryable<T>>? orderBy, string includeProperties = "");
    Task<T?> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task RemoveAsync(object id);
    void Remove(T entity);
    void Update(T entity);
}