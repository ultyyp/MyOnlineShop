using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Entities;
using OnlineShopBackend.Interfaces;

namespace OnlineShopBackend.Repositories
{
    public class ProductRepositoryEf : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepositoryEf(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProduct(Product product, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(product);

            await _dbContext.Products.AddAsync(product, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
        {
            return await _dbContext.Products.ToListAsync(cancellationToken);
        }

        public async Task<Product> GetProductById(Guid id, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(id);

            var prod = await _dbContext.Products.FirstAsync(p => p.Id == id, cancellationToken);
            return prod;
        }

        public async Task UpdateProductById(Guid id, Product product, CancellationToken cancellationToken)
        {
            var prod = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (prod != null)
            {
                _dbContext.Entry(prod).CurrentValues.SetValues(product);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NullReferenceException(nameof(prod));
            }
        }

        public async Task DeleteProductById(Guid id, CancellationToken cancellationToken)
        {
            var prod = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (prod != null)
            {
                _dbContext.Products.Remove(prod);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NullReferenceException(nameof(prod));
            }
        }





    }
}
