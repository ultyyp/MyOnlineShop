using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.EntityFramework.Repositories
{
	public class CartRepositoryEf : EfRepository<Cart>, ICartRepository
	{
		public CartRepositoryEf(AppDbContext dbContext) : base(dbContext) { }

		public Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken)
		{
			return Entities.Include(nameof(Cart.Items)).SingleAsync(x => x.AccountId == id, cancellationToken);
		}
	}
}
