namespace Domain.Abstractions
{
  public abstract class AbstractPresenter<TPassiveView> : IPresenter 
	where TPassiveView: IPassiveView
  {
	protected readonly TPassiveView _view;
	protected readonly ILogCommandInvoker _commandInvoker;

	public AbstractPresenter(TPassiveView view,ILogCommandInvoker invoker)
	{
	  _view = view ?? throw new ArgumentNullException(nameof(view));
	  _commandInvoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
	}

	public void Run()
	{
	  _view.Run();
	}
  }
}
