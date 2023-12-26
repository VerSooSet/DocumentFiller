namespace Database.Abstraction
{
   public interface ICommandInvoker
   {
	  Task ExecuteAsync<TCommandContext>(
		 TCommandContext c, 
		 CancellationToken cancellationToken=default)
		where TCommandContext : ICommandContext;
   }
}
