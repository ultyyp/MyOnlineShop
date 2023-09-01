using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Services
{
	public class CartService
	{
		private readonly IUnitOfWork _uow;

		public CartService(IUnitOfWork UOW)
		{
			_uow = UOW ?? throw new ArgumentNullException(nameof(UOW));
		}

		public virtual async Task AddProduct(Guid accountId, Product product, double quantity, CancellationToken cancellationToken)
		{
			if(product == null) throw new ArgumentNullException(nameof(product));
			if (quantity == 0) return;
			var cart = await _uow.CartRepository.GetCartByAccountId(accountId, cancellationToken);

			var existingItem = cart.Items.First(item => item.ProductId == product.Id);
			if(existingItem is null)
			{
				//Scenario 1:
				//Product Not Present In The Cart
				cart.Items.Add(new Cart.CartItem(Guid.Empty, product.Id, quantity));
			}
			else
			{
				//Scenario 2:
				//Product Present In The Cart
				existingItem.Quantity += quantity;
			}

			await _uow.CartRepository.Update(cart, cancellationToken);
			await _uow.SaveChangesAsync();
		}

		public virtual Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
		{
			return _uow.CartRepository.GetCartByAccountId(accountId, cancellationToken);
		}
	}
}
