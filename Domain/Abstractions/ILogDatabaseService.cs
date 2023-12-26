namespace Domain.Abstractions
{
   public interface ILogDatabaseService
   {
	  string LogString {get;}
	  Task LogToDatabase(string message, CancellationToken cancellationToken = default);
	}
}
