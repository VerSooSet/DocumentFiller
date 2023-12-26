using Domain.Abstractions;
using TemplateEngine.Docx;

namespace Domain.Models
{
    public class Template : ITemplate
    {
	  public string FilePath { get; set;}
	  
	  public Queue<FieldContent>? Fields { get; set;}

	  //dumb
	  public override bool Equals(object? obj)
	  { 
		 return true;
	  }
	  //dumb
	  public override int GetHashCode()
	  {
		 return this.GetHashCode();
	  }

    }
}
