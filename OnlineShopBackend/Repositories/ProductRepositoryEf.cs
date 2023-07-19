using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Entities;
using OnlineShopBackend.Interfaces;

namespace OnlineShopBackend.Repositories
{
    public class ProductRepositoryEf : EfRepository<Product>, IProductRepository
    {
        public ProductRepositoryEf(AppDbContext dbContext) : base(dbContext) { }

        public async Task DeleteProductById(Guid id, CancellationToken cancellationToken)
        {
            var prod = await Entities.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (prod != null)
            {
                Entities.Remove(prod);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new NullReferenceException(nameof(prod));
            }
        }

        public async Task UpdateProductById(Guid id, Product product, CancellationToken cancellationToken)
        {
            var prod = await Entities.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (prod != null)
            {
                Entities.Entry(prod).CurrentValues.SetValues(product);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new NullReferenceException(nameof(prod));
            }
        }
    }
}
