using OnlineShopBackend.Entities;

namespace OnlineShopBackend.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken);
    }
}
