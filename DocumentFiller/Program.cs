using Domain;
using Domain.Models;
using Domain.Persistence;
using Domain.Presenters;
using Domain.Services;

namespace TemplateDbLoader
{
   internal static class Program
   {
	  /// <summary>
	  ///  The main entry point for the application.
	  /// </summary>
	  [STAThread]
	  static void Main()
	  {
		 // To customize application configuration such as set high DPI settings or default font,
		 // see https://aka.ms/applicationconfiguration.
		 ApplicationConfiguration.Initialize();
		 		 
		  var presenter = new LoadPresenter(
			 new Form1(), 
			 new BaseService<Template>(),
			 new CommandInvoker(new LogTitleCommand(new DbTransactionProvider()) ),
			 new FakeMailService()
			);
		  presenter.Run();
	  }
   }
}