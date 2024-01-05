using Domain.Abstractions;
using Domain.Contexts;
using Domain.Models;

namespace Domain.Presenters
{
    public class LoadPresenter : AbstractPresenter<ILoadView>
    {
        private readonly ITemplateService _fService;
		private readonly IMailService _mailService;

		 public LoadPresenter(ILoadView view,
           ITemplateService fService,
		   ILogCommandInvoker invoker,
		   IMailService mailService)
		  :base(view,invoker)
         {
            _fService = fService ?? throw new ArgumentNullException(nameof(fService));
			_mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));

			_view.Open += () => LoadTemplate();
			_view.Save += () => SaveTemplate();
			_view.Send += () => SendOneEmail();
		 }
		 
        private void LoadTemplate()
        {
			string filePath = _view.FormFilePatch;
            if (filePath == null)
               throw new ArgumentNullException(nameof(filePath));
			if (filePath==String.Empty)
			   return;
			if (Path.GetExtension(filePath) != ".dotx")
			   throw new ArgumentException(nameof(filePath));
			
			OperationResult status = OperationResult.SourceProblem;
			
			try 
			{ 
				status = Task.Run(async () => await _fService.LoadTemplateAsync(filePath)).Result;
			}
			catch (Exception ex) 
			{ 
			   _commandInvoker.ExecuteAsync(
				  new LogCommandContext(ex.Message,"LoadTemplate",OperationResult.SourceProblem)
			   );
			}
			_view.TemplateContent = _fService.GetTemplateFieldsQueue();
			_view.OnLoadComplete();
			string logInfo = _fService.GetTemplateFileName();
			
			_commandInvoker.ExecuteAsync(
			   new LogCommandContext(logInfo,"LoadTemplate",status)
			 );
        }		  		 
		private void SaveTemplate()
        {
            var templateFileName = _fService.GetTemplateFileName();
			if (templateFileName == String.Empty)
			   throw new ArgumentException(nameof(templateFileName));

			var task = _fService.SaveTemplateAsync();
			if (task.Exception != null)
			{ 
			   _commandInvoker.ExecuteAsync(
				  new LogCommandContext
				  (
					 templateFileName + " " + task.Exception.Message,
					 "SaveTemplate",
					 OperationResult.SourceProblem
				  )
				);
			   return;
			}

			OperationResult result = task.Result ?? OperationResult.SourceProblem;
			
			if (!result.IsSuccess())
			{ 
				_commandInvoker.ExecuteAsync(
				  new LogCommandContext
				  (
					 templateFileName + " " + result.Message,
					 "SaveTemplate",
					 result
				  )
				);
			   return;
			}
			
			_commandInvoker.ExecuteAsync(
			   new LogCommandContext
			   (
				  templateFileName,
				  "SaveTemplate",
				  OperationResult.Success
			   )
			 );
        }
		 private void SendOneEmail()
		 {
            var templateFileName = _fService.GetTemplateFileName();
			if (templateFileName == String.Empty)
                throw new ArgumentException(nameof(templateFileName));

            _mailService.SendEmailAsync(
			   "test@jahoo.com",
			   String.Empty,
			   templateFileName
			   );
            
			string logText = "Attachment added to email. File name was: " + templateFileName;	

			_commandInvoker.ExecuteAsync(
			   new LogCommandContext(logText,"SendOneEmail",OperationResult.Success)
			 );
        }

    }
}
