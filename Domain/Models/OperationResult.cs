namespace Domain.Models
{
   public class OperationResult
   {
	  private bool isSuccess;
	  private int errorCode;
	  public string Message { get; private set; }
	  public bool IsSuccess()
	  {
		  return this.isSuccess;
	  }

	  public int GetErrorCode()
	  {
		  return this.errorCode;
	  }

	  public static readonly OperationResult Success
		  = new OperationResult() { 
			 isSuccess = true, 
			 errorCode = 0,
			 Message = String.Empty 
	  };

	  public static readonly OperationResult SourceProblem
		  = new OperationResult() { 
			 isSuccess = false, 
			 errorCode = 1,
			 Message = String.Empty 
	  };

	  public static readonly OperationResult AccessFileDenied
		  = new OperationResult() { 
			 isSuccess = false, 
			 errorCode = 2,
			 Message = "Access file denied." 
	  };

	  public static readonly OperationResult DatabaseError
		   = new OperationResult() { 
			  isSuccess = false, 
			  errorCode = 10,
			  Message = "Database error occurs at execution time." 
	  };

   }
}
