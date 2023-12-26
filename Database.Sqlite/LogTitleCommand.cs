using Dapper;
using Database.Abstraction;
using Domain.Contexts;
using System.Data.Common;

namespace Domain.Persistence
{
   public sealed class LogTitleCommand: IAsyncCommand<LogCommandContext>
	{
	  private readonly IDbTransactionProvider _provider;

	  public LogTitleCommand(IDbTransactionProvider provider)
	  {
		 _provider = provider ?? throw new ArgumentNullException(nameof(provider));
	  }

	  public async Task ExecuteAsync(
		 LogCommandContext context, 
         CancellationToken cancellationToken = default)
		 
	  { 
		 DbTransaction transaction = 
			await _provider.GetTransactionAsync(cancellationToken);
         DbConnection connection = transaction.Connection;
		 		 
		 try
		 {
			int operationId = 0;

			operationId = await connection.ExecuteScalarAsync<int>(@" 
				Select idOp from Operations
				 where Operations.nameOp like @nameOp",
				new { 
				   nameOp = context.Operation
				},
			   transaction);
		 
		   if (operationId == 0) 
		   { 
			operationId = await connection.ExecuteScalarAsync<int>(@"
			   Insert INTO Operations 
			   (nameOp) 
			   VALUES 
			   (@nameOp); 
			   SELECT last_insert_rowid();",
			   new { 
				  nameOp = context.Operation 
			   },
			   transaction);	
		   }
			await connection.ExecuteAsync(@"
				  INSERT INTO Logs
				   (
					   status,
					   idOp,
					   date,
					   logInfo
				   )
				   VALUES
				   (
					   @status,
					   @idOp,
					   datetime('now'),
					   @logInfo
				   );", 
				   new
				  {
					 status = context.OperationResult.Message,
					 idOp = operationId,
					 logInfo = context.LogInfo
				  }, transaction);
		 }
		 //dumb
		 catch (Exception ex)
		 {
		 }
		 finally
		 {
		   connection.Close();
		 }
	  }
   }
}
