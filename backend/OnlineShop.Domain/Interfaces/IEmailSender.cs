using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recepientEmail, string subject, string? body, CancellationToken cancellationToken);
    }
}
