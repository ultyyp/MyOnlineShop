using System.Runtime.Serialization;

namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : DomainException
    {
        public AccountNotFoundException(string message) : base(message)
        {
        }
    }
}