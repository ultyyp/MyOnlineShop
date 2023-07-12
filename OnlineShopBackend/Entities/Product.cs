namespace OnlineShopBackend.Entities
{
	public class Product
	{
		//For EF
		private Product() { }

		public Guid Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
	}
}
