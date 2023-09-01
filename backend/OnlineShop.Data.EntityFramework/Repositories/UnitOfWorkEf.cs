﻿using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Data.EntityFramework.Repositories
{
	public class UnitOfWorkEf : IUnitOfWork
	{
		public IAccountRepository AccountRepository { get; }
		public ICartRepository CartRepository { get; }
		public IProductRepository ProductRepository { get; }


		private readonly AppDbContext _dbContext;

		public UnitOfWorkEf(
			AppDbContext dbContext,
			IAccountRepository accountRepository,
			ICartRepository cartRepository,
			IProductRepository productRepository)
		{
			_dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
			AccountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
			CartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
			ProductRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
		}

		public Task SaveChangesAsync(CancellationToken cancellationToken = default)
			=> _dbContext.SaveChangesAsync(cancellationToken);
	}

}
