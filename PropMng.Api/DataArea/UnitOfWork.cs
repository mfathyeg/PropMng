using Gdnc.Services.UnitOfWorks;
using Microsoft.EntityFrameworkCore.Storage;
using PropMng.Api.data;
using System.Threading.Tasks;

namespace PropMng.Api.DataArea
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PropsMngDbContext context;
        private IDbContextTransaction transaction;

        public UnitOfWork(PropsMngDbContext context)
        {
            this.context = context;

            Persons = new RepositoryAsync<Person>(context);
            PersonsCorps = new RepositoryAsync<PersonsCorp>(context);
            Props = new RepositoryAsync<Prop>(context);
            PropsUnits = new RepositoryAsync<PropsUnit>(context);  
            Logs = new RepositoryAsync<Log>(context);
            LogsEnterances = new RepositoryAsync<LogsEnterance>(context);
            LogsTokens = new RepositoryAsync<LogsToken>(context);
            Invoices = new RepositoryAsync<Invoice>(context);
            Incomes = new RepositoryAsync<Income>(context);
        }
         
        public IRepositoryAsync<Person> Persons { get; }
        public IRepositoryAsync<PersonsCorp> PersonsCorps { get; }
        public IRepositoryAsync<Prop> Props { get; }
        public IRepositoryAsync<PropsUnit> PropsUnits { get; }
        public IRepositoryAsync<Log> Logs { get; }
        public IRepositoryAsync<LogsEnterance> LogsEnterances { get; }
        public IRepositoryAsync<LogsToken> LogsTokens { get; }
        public IRepositoryAsync<Invoice> Invoices { get; }
        public IRepositoryAsync<Income> Incomes { get; }


        public async Task<int> CompleteAsync() => await context.SaveChangesAsync();
        public int Complete() => context.SaveChanges();

        public async Task<int> CompleteAndCommitAsync()
        {
            var o = await CompleteAsync();
            await CommitAsync();
            return o;
        }
        public int CompleteAndCommit()
        {
            var o = Complete();
            Commit();
            return o;
        }

        public async Task BeginTransactionAsync() => transaction = await context.Database.BeginTransactionAsync();
        public void BeginTransaction() => transaction = context.Database.BeginTransaction();

        public async Task CommitAsync() => await transaction.CommitAsync();
        public void Commit() => transaction.Commit();

        public async Task RollbackAsync() => await transaction.RollbackAsync();
        public void Rollback() => transaction.Rollback();

        public void Dispose()
        {
            context.Dispose();
        }

    }
}
