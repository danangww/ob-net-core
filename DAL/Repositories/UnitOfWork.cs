using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly OnBoardingDanangDbContext dbContext;

        public IBaseRepository<Book> BookRepository { get; }

        public UnitOfWork(OnBoardingDanangDbContext context)
        {
            dbContext = context;

            BookRepository = new BaseRepository<Book>(dbContext);
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default)
        {
            return dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction StartNewTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> StartNewTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
