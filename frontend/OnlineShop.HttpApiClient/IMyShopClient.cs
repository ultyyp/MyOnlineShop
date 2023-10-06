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
        Task<CartResponse> GetCart(CancellationToken cancellationToken);
        Task AddCartItemToCart(AddCartItemRequest request, CancellationToken cancellationToken);
        Task DeleteCartItem(Guid id, CancellationToken cancellationToken);
        Task ReduceCartItemQuantity(Guid id, CancellationToken cancellationToken);
        Task IncreaseCartItemQuantity(Guid id, CancellationToken cancellationToken);
        Task UpdateProduct(Product product, Guid id, CancellationToken cancellationToken);
        Task<LoginByCodeResponse> Register(RegisterRequest account, CancellationToken cancellationToken);
        Task<LoginByCodeResponse> Login(LoginByPassRequest request, CancellationToken cancellationToken);
        public bool IsAuthorizationTokenSet { get; }
        public void SetAuthorizationToken(string token);
        public void ResetAuthorizationToken();
        public Task<AccountResponse> GetCurrentAccount(CancellationToken cancellationToken);
    }
}