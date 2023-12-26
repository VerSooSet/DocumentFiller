using TemplateEngine.Docx;

namespace Domain.Abstractions
{
   public interface ITemplate
   {	
	  public string FilePath {get; set;}
	  public Queue<FieldContent>? Fields { get; set;}
   }
}
