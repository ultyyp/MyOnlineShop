using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entities
{
	public class Cart : IEntity
	{
		public Cart(Guid id, Guid accountId)
		{
			Id = id;
			AccountId = accountId;
			Items = new List<CartItem>();
		}

		public Guid Id { get; init; }
		public Guid AccountId { get; set; }

		//Reverse Navigation Property
		public List<CartItem>? Items;

		public void AddItem(Guid productId, double quantity)
		{
			if (quantity <= 0) { throw new ArgumentOutOfRangeException(nameof(quantity)); }
			if (Items == null) { throw new InvalidOperationException("Cart items are null!"); }

			var existingItem = Items!.SingleOrDefault(item => item.ProductId == productId);

			if (existingItem is null)
			{
				Items!.Add(new Cart.CartItem(Guid.Empty, productId, quantity));
			}
			else
			{
				existingItem.Quantity += quantity;
			}
		}


		public record CartItem : IEntity
		{
			protected CartItem() { }

			public CartItem(Guid id, Guid productId, double quantity)
			{
				Id = id;
				ProductId = productId;
				Quantity = quantity;
			}

			public Guid Id { get; init; }

			//External key that incicates to Product.Id
			public Guid ProductId { get; init; }

			public double Quantity { get; set; }

            //External Key To A Cart.Id
            public Guid CartId { get; set; }
            public Cart Cart { get; set; }

			
		}
	}
}
