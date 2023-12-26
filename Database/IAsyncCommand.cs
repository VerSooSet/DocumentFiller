namespace Database.Abstraction
{
    public interface IAsyncCommand<in CCommandContext> where CCommandContext: ICommandContext
    {
	  Task ExecuteAsync(CCommandContext c, CancellationToken cancellationToken = default);
    }
}
