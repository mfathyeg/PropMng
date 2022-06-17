using PropMng.Api.data;
using PropMng.Api.DataArea;
using System;
using System.Threading.Tasks;

namespace Gdnc.Services.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryAsync<Person> Persons { get; }
        IRepositoryAsync<PersonsCorp> PersonsCorps { get; }
        IRepositoryAsync<Prop> Props { get; }
        IRepositoryAsync<PropsUnit> PropsUnits { get; }
        IRepositoryAsync<Log> Logs { get; }
        IRepositoryAsync<LogsEnterance> LogsEnterances { get; }
        IRepositoryAsync<LogsToken> LogsTokens { get; }
        IRepositoryAsync<Invoice> Invoices { get; }
        IRepositoryAsync<Income> Incomes { get; }

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task<int> CompleteAndCommitAsync();
        int Complete();
        Task<int> CompleteAsync();
        Task RollbackAsync();
        void Rollback();
        void Commit();
        void BeginTransaction();
        int CompleteAndCommit();
    }
}
