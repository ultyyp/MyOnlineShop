using MediatR;
using OnlineShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Events
{
    public class SendLogin2FA : INotification
    {
        public Account Account { get; }
        public ConfirmationCode Code { get; }

        public SendLogin2FA(Account account, ConfirmationCode code)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}
