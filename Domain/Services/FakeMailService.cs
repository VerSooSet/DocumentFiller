using Domain.Abstractions;
using System.Net.Mail;
using System.Threading;

namespace Domain.Services
{
   //dumb
    public class FakeMailService : IMailService
    {
        public async Task SendEmailAsync(
           string to,
           string subject,
           string body,
           CancellationToken cancellationToken,
           string? from = null,
           Attachment? template = null)
        {
            if (body.Length == 0)
                await Task.FromException(new ArgumentException(nameof(body)));

            if (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                cancellationToken.ThrowIfCancellationRequested();
            }
        }
    }
}
