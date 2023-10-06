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

		public virtual async Task AddProduct(Guid accountId, Guid productId, double quantity, CancellationToken cancellationToken)
		{
			var cart = await _uow.CartRepository.GetCartByAccountId(accountId, cancellationToken);
			cart.AddItem(productId, quantity);
			await _uow.CartRepository.Update(cart, cancellationToken);
			await _uow.SaveChangesAsync();
		}

		public virtual Task<Cart> GetAccountCart(Guid accountId, CancellationToken cancellationToken)
		{
			return _uow.CartRepository.GetCartByAccountId(accountId, cancellationToken);
		}

        public async Task DeleteCartItemById(Guid cartItemId, Guid accountId, CancellationToken cancellationToken)
        {
            var cart = await GetAccountCart(accountId, cancellationToken);
            var cartItem = cart.Items!.Single(it => it.Id == cartItemId);
			cart.Items!.Remove(cartItem);
            await _uow.CartRepository.Update(cart, cancellationToken);
            await _uow.SaveChangesAsync();
        }

        public async Task IncreaseCartItemQuantityById(Guid cartItemId, Guid accountId, CancellationToken cancellationToken)
        {
            var cart = await GetAccountCart(accountId, cancellationToken);
            var cartItem = cart.Items!.SingleOrDefault(it => it.Id == cartItemId);
			cartItem.Quantity++;
            await _uow.CartRepository.Update(cart, cancellationToken);
            await _uow.SaveChangesAsync();
        }

        public async Task DecreaseCartItemQuantityById(Guid cartItemId, Guid accountId, CancellationToken cancellationToken)
        {
			var cart = await GetAccountCart(accountId, cancellationToken);
            var cartItem = cart.Items!.Single(it => it.Id == cartItemId);
            
			if(cartItem.Quantity > 1)
			{
				cartItem.Quantity--;
			}
			else
			{
				await DeleteCartItemById(cartItemId, accountId, cancellationToken);
            }
            await _uow.CartRepository.Update(cart, cancellationToken);
            await _uow.SaveChangesAsync();
        }
    }
}
