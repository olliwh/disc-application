using class_library_disc.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace backend_disc.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DiscProfileDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DiscProfileDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            Console.WriteLine(entity);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int?> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return null;
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T?> Update(int id, T entity)
        {
            var existing = await _dbSet.FindAsync(id);
            if (existing == null) return null;

            //when AutoMapper maps UpdateCompanyDto to Company, it creates a new object 
            // When mapper creates entity, the Id will be 0 so we have to set it here
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(entity, id);
            }

            _context.Entry(existing).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
            return existing;
        }
    }
}
