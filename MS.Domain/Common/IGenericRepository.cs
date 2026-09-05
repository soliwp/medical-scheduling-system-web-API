using System.Linq.Expressions;

namespace MS.Domain.Common;
public interface IGenericRepository<T> where T : class
{
    Task CreateAsync (T entity);
    void UpdateAsync (T entity);
    Task<T> GetByIdAsync (int id);
    Task<List<T>> GetAllAsync();
    Task<bool> ExistAsync(Expression<Func<T,bool>> expression);
    Task SaveChangesAync();
}