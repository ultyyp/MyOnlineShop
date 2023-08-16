using System.Runtime.Serialization;

namespace OnlineShop.Domain.Exceptions
{
    [Serializable]
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string? message) : base(message)
        {
        }
    }
}