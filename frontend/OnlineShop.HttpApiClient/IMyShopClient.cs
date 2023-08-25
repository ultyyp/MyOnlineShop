using OnlineShop.HttpApiClient.Entities;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using System.Collections.Concurrent;

namespace OnlineShop.HttpApiClient
{
    public interface IMyShopClient
    {
        Task<IReadOnlyList<PageCounterResponse>> GetMetricsPathCounter(CancellationToken cancellationToken);
		Task AddProduct(Product product, CancellationToken cancellationToken);
        Task DeleteProduct(Guid id, CancellationToken cancellationToken);
        Task<Product> GetProduct(Guid id, CancellationToken cancellationToken);
        Task<List<Product>> GetProducts(CancellationToken cancellationToken);
        Task UpdateProduct(Product product, Guid id, CancellationToken cancellationToken);
        Task Register(RegisterRequest request, CancellationToken cancellationToken);
        Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken);
    }
}