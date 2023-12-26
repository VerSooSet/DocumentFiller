using Domain.Abstractions;
using Domain.Models;
using System.Text.RegularExpressions;
using TemplateEngine.Docx;
using Path = System.IO.Path;

namespace Domain.Services
{
   public class BaseService<TTemplate> : ITemplateService
        where TTemplate : ITemplate, new()
   {
	  protected TTemplate Template;

	  public BaseService()
	  {
	  }

	  public string GetTemplateFileName()
		 => Path.GetFileName(Template.FilePath);

	  public Queue<FieldContent>? GetTemplateFieldsQueue()
		 => Template.Fields;

	  public async Task<OperationResult> SaveTemplateAsync(CancellationToken token = default)
      {
		 var tcs = new TaskCompletionSource<OperationResult>(token);
		 
		 try { 

			if (Template.FilePath.Length == 0)
			   throw new ArgumentException(nameof(Template.FilePath));
			
			File.Delete("OutputDocument.docx");
            File.Copy(Template.FilePath, "OutputDocument.docx");

			using(var outputDocument = new TemplateProcessor("OutputDocument.doсx")
		 		  .SetRemoveContentControls(true))
			{
				Content data = new Content();
				data.Fields = (Template.Fields==null)? Template.Fields.ToArray() : new FieldContent[default];

		 		outputDocument.FillContent(data);
		 		outputDocument.SaveChanges();
				tcs.TrySetResult(OperationResult.Success);
			}
		 }
		 catch(Exception ex)
		 { 
			tcs.TrySetException(ex);
		 }
		 finally
		 { 
			await tcs.Task; 
		 }
		 
		 return await tcs.Task;
      }
	  public async Task<OperationResult> LoadTemplateAsync(string filePath, CancellationToken token = default)
	  {
		 var tcs = new TaskCompletionSource<OperationResult>(token);

         if (filePath.Length == 0)
         {
             tcs.TrySetException(new ArgumentException(nameof(filePath)));
			 return await tcs.Task;
         }

		 string source = String.Empty;
		   
		 try { 
			using (var outputDocument = new TemplateProcessor(filePath)
					  .SetRemoveContentControls(true))
			{
			   source = ((System.Xml.Linq.XElement)
					 outputDocument.Document
								 .Document
									.Document
									.FirstNode).Value;
			}
		 }
		 catch (Exception ex) 
		 { 
			tcs.TrySetException(ex);
		 }
		 var dataQueue = SplitToData(source);
		
		 //dumb
		 Queue<FieldContent> fieldsQueue = 
		   new Queue<FieldContent>(dataQueue.Select(
		 		   x => new FieldContent("FORMTEXT",x))
		 		.ToList()
			); 

		 Template = new TTemplate();
		 Template.FilePath = filePath;
		 Template.Fields = fieldsQueue;
		
         tcs.TrySetResult(OperationResult.Success);

         return await tcs.Task;
	  }
      private Queue<string> SplitToData(string origin)
	  { 
		 var array =  new string[] {"{\\{[^}]*\\}"} 
			.SelectMany(x => Regex.Matches(origin, x, RegexOptions.Multiline)
			.Cast<Match>().Where(m => m.Success)
			.Select(m => m.Value)).ToArray();
		 return new Queue<string>(array);
	  }
   }
}