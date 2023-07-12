using OnlineShopBackend.Entities;

namespace OnlineShopBackend.Interfaces
{
    public interface IProductRepository
    {

        public Task AddProduct(Product product, CancellationToken cancellationToken);

        public Task<List<Product>> GetProducts(CancellationToken cancellationTokn);

        public Task<Product> GetProductById(Guid id, CancellationToken cancellationToken);

        public Task UpdateProductById(Guid id, Product product, CancellationToken cancellationToken);

        public Task DeleteProductById(Guid id, CancellationToken cancellationToken);


    }
}
