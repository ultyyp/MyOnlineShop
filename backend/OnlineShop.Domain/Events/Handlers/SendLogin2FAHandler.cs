using MediatR;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Events.Handlers
{
    public class SendLogin2FAHandler : INotificationHandler<SendLogin2FA>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<SendLogin2FAHandler> _logger;

        public SendLogin2FAHandler(
            IEmailSender emailSender,
            ILogger<SendLogin2FAHandler> logger)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        

        public async Task Handle(SendLogin2FA notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("2FA Code Sent To Email {Email}: 2FA = {CodeCode}", notification.Account.Email, notification.Code.Code);
            await _emailSender.SendEmailAsync(
                notification.Account.Email!,
                "2FA Code",
                notification.Code.Code,
                cancellationToken);
        }
    }
}
