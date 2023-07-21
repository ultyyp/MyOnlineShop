namespace OnlineShopBackend.Data
{
	using Microsoft.EntityFrameworkCore;
	using OnlineShopBackend.Entities;

	public class AppDbContext : DbContext
	{
		//Список таблиц:
		public DbSet<Product> Products => Set<Product>();

        public DbSet<Account> Accounts => Set<Account>();

        public AppDbContext(
			DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}
	}

}
