using Microsoft.EntityFrameworkCore;
using StatSanctum.Contexts;
using StatSanctum.Entities;
using StatSanctum.Helpers;

namespace StatSanctum.Repositories
{
    public class Repository<T> : IRepository<T> where T : Base
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        async Task<T> IRepository<T>.CreateAsync(T dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            
            await _dbSet.AddAsync(dto);
            await _context.SaveChangesAsync(); // Save to DB

            return dto;
        }

        async Task<bool> IRepository<T>.DeleteByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(T)} with ID {id} not found.");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        async Task<IEnumerable<T>> IRepository<T>.GetAllAsync() => await _dbSet.ToListAsync();

        async Task<T> IRepository<T>.GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"{typeof(T)} with ID {id} not found.");
            }
            return entity;
        }

        async Task<T> IRepository<T>.UpdateAsync(T dto)
        {
            _dbSet.Update(dto);
            await _context.SaveChangesAsync();
            return dto;
        }
    }
}
