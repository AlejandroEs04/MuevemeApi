using Microsoft.EntityFrameworkCore;

namespace MuevemeApi.Data;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly DataContextEF _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DataContextEF context) 
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task<bool> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if(entity != null) 
        {
            _dbSet.Remove(entity);
            return await SaveChangesAsync();
        }
        return false;
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}