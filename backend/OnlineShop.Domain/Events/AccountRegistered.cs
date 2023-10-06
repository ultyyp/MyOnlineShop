using MediatR;
using OnlineShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Events
{
    public class AccountRegistered : INotification
    {
        public Account Account { get; }
        public DateTime RegisteredAt { get; }

        public AccountRegistered(Account account, DateTime registeredAt)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            RegisteredAt = registeredAt;
        }
    }
}
