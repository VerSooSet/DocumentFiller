using Database.Abstraction;
using Domain.Abstractions;
using Domain.Contexts;

namespace Domain
{
    public class CommandInvoker: ILogCommandInvoker
   {
	  private readonly IAsyncCommand<LogCommandContext> _cmd;

	  public CommandInvoker(IAsyncCommand<LogCommandContext> cmd)
	  { 
		_cmd = cmd ?? throw new ArgumentNullException(nameof(cmd));
	  }

	  public async Task ExecuteAsync(
		 LogCommandContext co, 
		 CancellationToken cancellationToken = default)
	  {
		 await _cmd.ExecuteAsync(co,cancellationToken);
	  }
   }   
}
