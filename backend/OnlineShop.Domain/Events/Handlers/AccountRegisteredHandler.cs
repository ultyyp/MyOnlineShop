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
    public class AccountRegisteredHandler : INotificationHandler<AccountRegistered>
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountRegisteredHandler> _logger;

        public AccountRegisteredHandler(
            IEmailSender emailSender,
            ILogger<AccountRegisteredHandler> _logger)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            this._logger = _logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task Handle(AccountRegistered notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Account Registration Handled");
            await _emailSender.SendEmailAsync(
                notification.Account.Email,
                "Confirming Registration",
                "You have succesfully registed!",
                cancellationToken);
        }
    }
}
