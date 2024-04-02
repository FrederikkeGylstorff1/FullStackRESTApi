 using Microsoft.EntityFrameworkCore;
 using System;

namespace RestApiLK.services
{
   

    public abstract class BaseDBContext<T> where T : class, new()
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        protected BaseDBContext(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        public virtual async Task<T> InsertAsync(T data)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _dbSet.AddAsync(data);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return data; // Returner det oprettede dataobjekt
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        public virtual async Task<List<T>> GetAllAsync()
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _dbSet.ToListAsync();
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _dbSet.FindAsync(id);
                    await transaction.CommitAsync();
                    return result;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public virtual async Task<T> UpdateAsync(T updatedData)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbSet.Update(updatedData);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return updatedData;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        public virtual async Task<bool> DeleteAsync(int id)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var entity = await _dbSet.FindAsync(id);
                    if (entity != null)
                    {
                        _dbSet.Remove(entity);
                        var result = await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return result > 0;
                    }
                    return false;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        // You can add abstract methods here if needed
    }

}
