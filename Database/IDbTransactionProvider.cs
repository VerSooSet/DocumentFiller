using System.Data.Common;

namespace Database.Abstraction
{
   public interface IDbTransactionProvider
   {
	  Task<DbTransaction> GetTransactionAsync(CancellationToken cancellationToken = default);
   }
}
