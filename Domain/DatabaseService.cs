
using Database;
using Domain.Abstractions;

namespace Domain
{
   public class DatabaseService : ILogDatabaseService
   {
	  private readonly DbTransactionProvider provider;

	  public string LogString => throw new NotImplementedException();

	  public DatabaseService(DbTransactionProvider provider)
	  {
		 this.provider = provider;
	  }

	  public Task LogToDatabase(string message, CancellationToken cancellationToken = default)
	  {
		 throw new NotImplementedException();
	  }

   }
}
