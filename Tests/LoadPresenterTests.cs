using Domain.Abstractions;
using Domain.Models;
using Domain.Presenters;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Net.Mail;
using TemplateEngine.Docx;

namespace Tests
{
   public class LoadPresenterTests
   {
	  private ILoadView view;
	  
	  private ITemplateService _fService;
	  private ILogCommandInvoker _commandInvoker;
	  private IMailService _mailService;
	  	  
	  [SetUp]
	  public void Setup()
	  {
		view = Substitute.For<ILoadView>();
		
		_fService = Substitute.For<ITemplateService>();
		_commandInvoker = Substitute.For<ILogCommandInvoker>();
		_mailService = Substitute.For<IMailService>();
		_fService.LoadTemplateAsync(Arg.Any<string>())
			.Returns(x => OperationResult.Success);
		_fService.SaveTemplateAsync()
			.Returns(x => OperationResult.Success);
		var presenter = new LoadPresenter(view, _fService, _commandInvoker,_mailService);
		presenter.Run();
	  }

	  [Test]
	  public void OpeningUnSupportedTemplateFile()
	  {
		 view.FormFilePatch.Returns("../Test.txt");
		 _fService.GetTemplateFieldsQueue().Returns(
			new Queue<FieldContent>(new[]{ 
			   new FieldContent("name1","value1"), 
			   new FieldContent() 
			})
		 );
		 
		 var act = ()=> view.Open += Raise.Event<Action>();
		 
		 Assert.IsNull(view.TemplateContent);
	  }

	  [Test]
	  public void SuccessOpeningTemplateFile()
	  {
		 view.FormFilePatch.Returns("../Test.dotx");
		 _fService.GetTemplateFieldsQueue().Returns(
			new Queue<FieldContent>(new[]{ 
			   new FieldContent("name1","value1"), 
			   new FieldContent() 
			})
		 );

		 var act = ()=> view.Open += Raise.Event<Action>();
		 
		 Assert.DoesNotThrow(act.Invoke);
		 Assert.IsNotNull(view.TemplateContent);
	  }
	    
	  [Test]
	  public void UnAcceptableTemplateToSave()
	  {
		_fService.GetTemplateFileName().Returns("");
		
		Assert.Throws<ArgumentException>(()=> view.Save += Raise.Event<Action>());
	  }
	  
	  [Test]
	  public void SuccessSaveTemplate()
	  { 
		 var val = _fService.GetTemplateFileName().Returns("test_incoming.dotx");
		 view.Save += Raise.Event<Action>();
		
		 var act = ()=> view.Save += Raise.Event<Action>();
		 
		 Assert.Multiple(()=>{ 
			Assert.DoesNotThrow(act.Invoke);
			Assert.NotNull((_fService.SaveTemplateAsync().Result));
		 });
	  }

	  [Test]
	  public void ErrorOccursDueSendingEmail()
	  { 
		 _fService.GetTemplateFileName().Returns(String.Empty);

		 var act = ()=> view.Send += Raise.Event<Action>();

		 Assert.Throws<ArgumentException>(act.Invoke);
	  }

	  [Test]
	  public void SuccessSendingEmail()
	  { 
		 _fService.GetTemplateFileName().Returns("test.dotx");

		 var act = ()=> view.Send += Raise.Event<Action>();

		 Assert.DoesNotThrow(act.Invoke);
	  }

   }
}