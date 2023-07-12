namespace OnlineShopBackend.Data
{
	using Microsoft.EntityFrameworkCore;
	using OnlineShopBackend.Entities;

	public class AppDbContext : DbContext
	{
		//Список таблиц:
		public DbSet<Product> Products => Set<Product>();

		public AppDbContext(
			DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
	}

}
