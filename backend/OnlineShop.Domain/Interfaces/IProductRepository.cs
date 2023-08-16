using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        public Task UpdateProductById(Guid id, Product product, CancellationToken cancellationToken);
        public Task DeleteProductById(Guid id, CancellationToken cancellationToken);
    }
}
