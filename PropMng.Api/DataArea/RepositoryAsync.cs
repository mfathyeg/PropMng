using Microsoft.EntityFrameworkCore;
using PropMng.Api.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PropMng.Api.DataArea
{
    public class RepositoryAsync<T> :
       IRepositoryAsync<T> where T : class
    {
        private readonly PropsMngDbContext _db;
        public RepositoryAsync(PropsMngDbContext db)
        {
            _db = db;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            IQueryable<T> q = _db.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    q = q.Include(include);
            return q.Where(predicate);
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> dbSet = _db.Set<T>();

            IQueryable<T> query = null;
            foreach (var include in includes)
            {
                query = (query ?? dbSet).Include(include);
            }
            return query ?? dbSet;
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) => await _db.Set<T>().AnyAsync(predicate);

        public virtual IQueryable<T> GetAll() => _db.Set<T>();

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) => _db.Set<T>().Where(predicate);

        public async Task<T> FindAsync(object id) => await _db.Set<T>().FindAsync(id);

        public virtual void AddAsync(T entity) => _db.Set<T>().Add(entity);

        public virtual int AddRangeAsync(IEnumerable<T> entities)
        {
            _db.Set<T>().AddRange(entities);
            return entities.Count();
        }
        public virtual void RemoveAsync(T entity) => _db.Entry(entity).State = EntityState.Deleted;

        public virtual void RemoveAsync(object id) => _db.Entry(FindAsync(id)).State = EntityState.Deleted;

        public virtual void EditAsync(T entity) => _db.Entry(entity).State = EntityState.Modified;

        public virtual int EditRangeAsync(IEnumerable<T> entities)
        {
            _db.Set<T>().UpdateRange(entities);
            return entities.Count();
        }

        private bool _disposed;

        protected async virtual Task DisposeAsync(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    await _db.DisposeAsync();

            _disposed = true;
        }

        public async ValueTask DisposeAsync() => await DisposeAsync(true);

        public virtual int RemoveRangeAsync(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
            return entities.Count();
        }
        public async virtual Task SaveAsync() => await _db.SaveChangesAsync();

        public async Task ExecuteSqlCommandAsync(string command, params object[] paramters) => await _db.Database.ExecuteSqlRawAsync(command, paramters);
    }
}