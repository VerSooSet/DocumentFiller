using Domain.Models;
using TemplateEngine.Docx;

namespace Domain.Abstractions
{
    public interface ITemplateService
    {
	   public Task<OperationResult> LoadTemplateAsync(
		 string filepath,
		 CancellationToken token = default);

	   public Task<OperationResult> SaveTemplateAsync(
		 CancellationToken token = default);
		 		
	  string GetTemplateFileName();
	  Queue<FieldContent>? GetTemplateFieldsQueue();
	 }
}
