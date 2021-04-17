using Hahn.ApplicationProcess.December2020.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.December2020.Data.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _ctx;
        private readonly DbSet<T> _dbSet;

        protected BaseRepository(AppDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<T>();
        }

        public async Task Delete(int? id)
        {
            if (await Exist(id))
            {
                var entity = await Find(id);
                _ctx.Remove(entity);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task<bool> Exist(int? id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null;
        }

        public async Task<T> Find(int? id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> Count()
        {
            return await _dbSet.CountAsync();
        }

        public async Task Save(T t)
        {
            if (t != null)
            {
                await _dbSet.AddAsync(t);
                await _ctx.SaveChangesAsync();
            }
        }

        public async Task Update(T t)
        {
            if (t != null)
            {
                _ctx.Update(t);
                await _ctx.SaveChangesAsync();
            }
        }
    }
}
