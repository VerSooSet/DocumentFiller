using System.ComponentModel;
using TemplateEngine.Docx;

namespace Domain.Abstractions
{
   public interface ILoadView: IPassiveView
   {
	  public string FormFilePatch { get; }
	  public Queue<FieldContent> TemplateContent{ get; set;}
	  
	  event Action Open;
	  event EventHandler OpenComplete;
	  event Action Save;
	  event Action Send;
	 	  
	  public void DrawFormElements();
	  
	  public void OnLoadComplete();
   }
}
