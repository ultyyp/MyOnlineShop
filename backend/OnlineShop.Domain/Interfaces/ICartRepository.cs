using OnlineShop.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
	public interface ICartRepository : IRepository<Cart>
	{
		Task<Cart> GetCartByAccountId(Guid id, CancellationToken cancellationToken);
	}
}
