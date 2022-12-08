using GameShopStore.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GameShopStore.Infrastructure.Repos
{
    public abstract class BaseRepo<T> : IBaseRepository<T>
        where T: class
    {
        protected readonly ApplicationDbContext _appDbCtx;

        public BaseRepo(ApplicationDbContext appDbCtx)
        {
            _appDbCtx = appDbCtx;
        }

        public async void Add(T entity)
        {
            await _appDbCtx.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _appDbCtx.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _appDbCtx.Set<T>().RemoveRange(entities);
        }

        public async Task DeleteRangeAsync(Expression<Func<T, bool>> expression)
        {
            var entitiesToDelete = await _appDbCtx.Set<T>().Where(expression).ToListAsync();
            _appDbCtx.Set<T>().RemoveRange(entitiesToDelete);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> expression)
        {
            return await _appDbCtx.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _appDbCtx.Set<T>().Where(expression).ToListAsync();
        }
        public virtual async Task<T> GetAsync(int id)
        {
            return await _appDbCtx.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _appDbCtx.Set<T>().ToListAsync();
        }

        public void Update(T entity)
        {
            _appDbCtx.Set<T>().Update(entity);
        }

        public async Task<IEnumerable<T>> GetAllOrderedByAsync<TKey>(Expression<Func<T, TKey>> expression)
        {
            return await _appDbCtx.Set<T>().OrderBy(expression).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllOrderedByDescAsync<TKey>(Expression<Func<T, TKey>> expression)
        {
            return await _appDbCtx.Set<T>().OrderByDescending(expression).ToListAsync();
        }

        public async Task<T> GetLatestAsync()
        {
            var records = await _appDbCtx.Set<T>().ToListAsync();

            return records.LastOrDefault();
        }
    }
}
