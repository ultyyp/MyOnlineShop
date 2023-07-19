using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Entities;
using OnlineShopBackend.Interfaces;

namespace OnlineShopBackend.Repositories
{
    public class AccountRepositoryEf : EfRepository<Account>, IAccountRepository
    {
        public AccountRepositoryEf(AppDbContext dbContext) : base(dbContext) { }

        public Task<Account> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (email is null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return Entities.SingleAsync(x => x.Email == email, cancellationToken);
        }
    }
}
