using Microsoft.EntityFrameworkCore;
using CheapUdemy.Contracts;
using CheapUdemy.Data;

namespace CheapUdemy.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MyAppDbContext _context;
        private readonly MyAppDbContext _ctx;

        public GenericRepository(MyAppDbContext context)
        {
            this._context = context;
            this._ctx = context;
        }



        public virtual async Task<T> AddAsync(T entity)
        {
            await _ctx.AddAsync(entity);
            await _ctx.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            T? entity = await this.GetAsync(id);
            _ctx.Set<T>().Remove(entity);
            await _ctx.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            T? entity = await this.GetAsync(id);
            return entity != null;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _ctx.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int? id)
        {
            if (id is null) return null;

            return await _ctx.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _ctx.Update(entity);
            await _ctx.SaveChangesAsync();
        }
    }
}
