using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PropMng.Api.DataArea
{
    public interface IRepositoryAsync<T> : IAsyncDisposable where T : class
    {

        IQueryable<T> GetAll();

        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
        Task<T> FindAsync(object id);

        void AddAsync(T entity);
        int AddRangeAsync(IEnumerable<T> entities);
        void RemoveAsync(T entity);
        int RemoveRangeAsync(IEnumerable<T> entities);
        void RemoveAsync(object id);

        void EditAsync(T entity);
        int EditRangeAsync(IEnumerable<T> entities);

        Task SaveAsync();
        Task ExecuteSqlCommandAsync(string command, params object[] paramters);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params string[] includes);
    }
}
