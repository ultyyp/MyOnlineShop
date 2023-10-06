using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Exceptions;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OnlineShop.Data.EntityFramework.Repositories
{
	public class CartRepositoryEf : EfRepository<Cart>, ICartRepository
	{
		public CartRepositoryEf(AppDbContext dbContext) : base(dbContext) { }

        public async Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken)
        {
            var cart = await Entities
                .Include(it => it.Items)
                .SingleOrDefaultAsync(it => it.AccountId == id, cancellationToken);

            if (cart is null)
            {
                throw new AccountNotFoundException("Account with given email not found");
            }

            return cart;
        }

        


    }
}
