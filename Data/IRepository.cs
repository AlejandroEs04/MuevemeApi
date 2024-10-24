namespace MuevemeApi.Data;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<bool> AddAsync (T entity);
    Task<bool> UpdateAsync (T entity);
    Task<bool> DeleteAsync (int id);
    Task<bool> SaveChangesAsync();
}