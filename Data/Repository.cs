using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Lab.DotNetEfCore.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context!.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await ExecuteInTransactionAsync(async () =>
            {
                _context.Set<T>().Add(entity);
                await _context.SaveChangesAsync();
            });
        }

        public async Task UpdateAsync(T entity)
        {
            await ExecuteInTransactionAsync(async () =>
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
            });
        }

        public async Task DeleteAsync(int id)
        {
            await ExecuteInTransactionAsync(async () =>
            {
                var entity = await _context.Set<T>().FindAsync(id);
                if (entity != null)
                {
                    _context.Set<T>().Remove(entity);
                    await _context.SaveChangesAsync();
                }
            });
        }

        private async Task ExecuteInTransactionAsync(Func<Task> operation, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(isolationLevel);
            try
            {
                await operation();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}

