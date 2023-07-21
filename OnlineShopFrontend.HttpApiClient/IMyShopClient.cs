using OnlineShopFrontend.HttpApiClient.Entities;

namespace OnlineShopFrontend.HttpApiClient
{
    public interface IMyShopClient
    {
        Task AddProduct(Product product, CancellationToken cancellationToken);
        Task DeleteProduct(Guid id, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> GetProducts(CancellationToken cancellationToken);
        Task UpdateProduct(Product product, Guid id, CancellationToken cancellationToken);
        Task Register(Account account, CancellationToken cancellationToken);
    }
}