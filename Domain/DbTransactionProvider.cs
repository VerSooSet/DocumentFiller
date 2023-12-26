using Database;
using Database.Abstraction;
using System.Data.Common;
using System.Data.SQLite;

namespace Domain
{
   public class DbTransactionProvider: IDbTransactionProvider, IDisposable
   {
		private DbConnection _connection;
        private DbTransaction _transaction;
                
        protected bool _disposed;
	          
        public bool IsInitialized => ! _disposed && _transaction != null;
               
        public async Task<DbTransaction> GetTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(DbTransactionProvider));
            
            if (_transaction != null)
                return _transaction;
           
            _connection = await GenConnectionAsync(cancellationToken);
            _transaction = await _connection.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);
            return _transaction;
        }
	    ~DbTransactionProvider() => Dispose(false);

		private async Task<DbConnection> GenConnectionAsync(CancellationToken cancellationToken = default)
		{
			DbConnection conn = new SQLiteConnection(Helper.GetConnectionString());
            await conn.OpenAsync(cancellationToken);
            return conn;
		}

        protected void ComminTransaction()
        {
            if (!this.IsInitialized)
                return;
            _transaction.Commit();
        }

        protected void RollbackTransaction()
        {
            if (!this.IsInitialized)
                return;
            _transaction.Rollback();
        }

		protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            _disposed = true;

            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

   }
}