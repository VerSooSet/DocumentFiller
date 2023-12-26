using System.Net.Mail;

namespace Domain.Abstractions
{
   public interface IMailService
   {
	  Task SendEmailAsync(
		 string to, 
		 string subject, 
		 string body,
		 CancellationToken cancellationToken = default,
		 string? from = null,
		 Attachment? template = null
		 );
	  
   }
}
