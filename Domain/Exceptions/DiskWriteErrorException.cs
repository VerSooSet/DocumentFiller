namespace Domain.Exceptions
{
   public class DiskWriteErrorException: Exception
   {
	  public DiskWriteErrorException()
            : base($"Disk Write Error")
        {
        }
   }
}
