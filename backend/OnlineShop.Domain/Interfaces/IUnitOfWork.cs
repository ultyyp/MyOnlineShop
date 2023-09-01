using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
	public interface IUnitOfWork
	{
		IAccountRepository AccountRepository { get; }
		ICartRepository CartRepository { get; }
		IProductRepository ProductRepository { get; }
		Task SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
