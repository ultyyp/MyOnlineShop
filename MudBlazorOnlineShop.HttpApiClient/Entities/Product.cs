namespace OnlineShopFrontend.Entities
{
	public class Product
	{
		//For EF
		public Product() {}

		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}
