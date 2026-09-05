using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MS.Domain.Common;
using System.Linq.Expressions;

namespace MS.Infrastructure.EF.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly MSContext _context;

    public GenericRepository(MSContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task<bool> ExistAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().Where(expression).AnyAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }


    public void UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
    }
    public async Task SaveChangesAync()
    {
        await _context.SaveChangesAsync();
    }
}
