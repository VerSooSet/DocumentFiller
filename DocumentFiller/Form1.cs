using Domain.Abstractions;
using TemplateEngine.Docx;

namespace TemplateDbLoader
{
   public partial class Form1 : Form, ILoadView
   {
	  private bool drawMark = false;
	  public string FormFilePatch { get => GetFilePath() ?? String.Empty; }
	  public Queue<FieldContent> TemplateContent {get;set; }

	  public event Action Open;

	  public event EventHandler OpenComplete;
	 
	  public event Action Save;
	  
	  public event Action Send;
 
	  public Form1()
	  {
		 InitializeComponent();
		 
		 button1.Click += (sender, args) => Invoke(Open);
		 
		 this.OpenComplete += (sender, args) => { 
			
			button2.Enabled= true;
			button3.Enabled= true;
			
			DrawFormElements();
			button1.Enabled= false;
		 };

		 button2.Click += (sender, args) => Invoke(Save);
		 button3.Click += (sender, args) => Invoke(Send);
	  }	  	   
	  public void Run()
	  {
		 Application.Run(this);
	  }

	 public void OnLoadComplete()
	 {
        OnLoadComplete(new EventArgs());
	 }

	 protected void OnLoadComplete(EventArgs e)
	 {
       EventHandler handler = OpenComplete;
       if (handler != null)
           handler(this, e);
	 }

	 public void DrawFormElements()
	 {
		 if (drawMark)
			return;
		 
		 var values = TemplateContent;

		if (values==null)
			throw new ArgumentNullException(nameof(values));
		 		 
		if (values.Count() == 0)
			return;
		 
		if (values.Count() > 20)
			throw new ArgumentOutOfRangeException(nameof(values));

		const int posY = 20;
		 
		for(int x = 0; x < values.Count(); x++)
		{ 
		   this.Controls.Add (
			  new Label() { 
				 Name = x + "_Label", 
				 Text = values.ElementAt(x).Value, 
				 Location = new System.Drawing.Point(50, posY + 30 * x ), 
				 Size = new Size(145, 15) 
			  }
			);
		
			this.Controls.Add (
			  new TextBox() { 
				 Name = x + "_Textbox", 
				 Location = new System.Drawing.Point(250, posY + 30 * x ), 
				 Size = new Size(145, 15) 
			  }
			);
		}
		 drawMark = true;
	  }
	  private new void Invoke(Action action)
      {
          if (action != null) action();
      } 
	  private string GetFilePath()
	  { 
		 using (OpenFileDialog openFileDialog = new OpenFileDialog())
		 {
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "dotx files (*.dotx)|*.dotx";
			
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			   return openFileDialog.FileName;
			
			return String.Empty;
		 }
	  }
   }
}