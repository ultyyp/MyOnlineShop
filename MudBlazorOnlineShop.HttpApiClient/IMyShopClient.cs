using OnlineShopFrontend.Entities;

namespace OnlineShopFrontend.Interfaces
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task DeleteProduct(long id, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> GetProducts(CancellationToken cancellationToken);
        Task UpdateProduct(Product product, long id, CancellationToken cancellationToken;
    }
}