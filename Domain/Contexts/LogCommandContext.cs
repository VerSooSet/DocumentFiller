using Database.Abstraction;
using Domain.Models;

namespace Domain.Contexts
{
   public class LogCommandContext : ICommandContext
    {
        public string LogInfo { get; }
        public string Operation { get; }
	    
		public OperationResult OperationResult {get;}

        public LogCommandContext(string log,
           string operationName,
		   OperationResult operationResult)
        {
            LogInfo = log ?? string.Empty;
            Operation = operationName ?? string.Empty;
            OperationResult = operationResult;
        }
    }
}
