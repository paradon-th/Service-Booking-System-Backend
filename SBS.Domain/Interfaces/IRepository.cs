using System.Linq.Expressions;

namespace SBS.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, string? includeProperties = null);
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
