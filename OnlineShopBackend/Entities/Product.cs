using OnlineShopBackend.Interfaces;

namespace OnlineShopBackend.Entities
{
	public class Product : IEntity
	{
        private Product() { }

        public Product(string name, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }
       
        public Guid Id { get; init; }
		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}
