﻿namespace OnlineShop.Data.EntityFramework
{
	using Microsoft.EntityFrameworkCore;
	using OnlineShop.Domain.Entities;
	using static OnlineShop.Domain.Entities.Cart;

	public class AppDbContext : DbContext
	{
		public DbSet<Product> Products => Set<Product>();
        public DbSet<Account> Accounts => Set<Account>();
		public DbSet<Cart> Carts => Set<Cart>();
		public DbSet<CartItem> CartItems => Set<CartItem>();

		public AppDbContext(
			DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			BuildAccountModel(modelBuilder);
		}

		private static void BuildAccountModel(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Account>()
				.Property(e => e.Roles)
				.HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
						.Select(Enum.Parse<Role>).ToArray()
				);
		}
	}

}
