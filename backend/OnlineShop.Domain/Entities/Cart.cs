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
		public Guid Id { get; init; }
		public Guid AccountId { get; set; }

		//Reverse Navigation Property
		public List<CartItem>? Items;


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
			public Cart Cart { get; set; } = null!;
		}
	}
}
