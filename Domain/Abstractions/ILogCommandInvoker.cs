using Database.Abstraction;
using Domain.Contexts;

namespace Domain.Abstractions
{
   public interface ILogCommandInvoker
   {
	  Task ExecuteAsync(LogCommandContext c, 
		 CancellationToken cancellationToken=default);
   }
}
