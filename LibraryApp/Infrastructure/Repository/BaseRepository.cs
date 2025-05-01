using LibraryApp.Infrastructure.Repository.Interface;
using LibraryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Infrastructure.Repository
{
    namespace LibraryApp.Infrastructure.Repository
    {
        public class BaseRepository<T> : IRepository<T> where T : class
        {
            protected readonly AppDbContext _context;
            protected readonly DbSet<T> _dbSet;

            public BaseRepository(AppDbContext context)
            {
                _context = context;
                _dbSet = _context.Set<T>();
            }

            public virtual async Task<IEnumerable<T>> GetAllAsync()
                => await _dbSet.ToListAsync();

            public virtual async Task<T?> GetByIdAsync(int id)
                => await _dbSet.FindAsync(id);

            public virtual async Task AddAsync(T obj)
            {
                await _dbSet.AddAsync(obj);
                await _context.SaveChangesAsync();
            }

            public virtual async Task UpdateAsync(T obj)
            {
                _dbSet.Update(obj);
                await _context.SaveChangesAsync();
            }

            public virtual async Task DeleteAsync(int id)
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity != null)
                {
                    _dbSet.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }

            
        }
    }

}
