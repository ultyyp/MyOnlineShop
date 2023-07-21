using OnlineShopFrontend.HttpApiClient.Entities;
using System.Net.Http.Json;

namespace OnlineShopFrontend.HttpApiClient
{
    public class MyShopClient : IDisposable, IMyShopClient
	{
		private readonly string _host;
		private readonly HttpClient _httpClient;

		public MyShopClient(string host = "http://myshop.com/", HttpClient? httpClient = null)
		{
			ArgumentNullException.ThrowIfNull(host, nameof(host));

			if (!Uri.TryCreate(host, UriKind.Absolute, out var hostUri))
			{
				throw new ArgumentException("The host address should be a valud url", nameof(host));
			}
			_host = host;
			_httpClient = httpClient ?? new HttpClient();
			if (_httpClient.BaseAddress is null)
			{
				_httpClient.BaseAddress = hostUri;
			}
		}


		public async Task<Product> GetProduct(Guid id, CancellationToken cancellationToken = default)
		{
			string uri = $"/catalog/get_product?productId={id}";
			Product? product = await _httpClient.GetFromJsonAsync<Product>(uri, cancellationToken);
			return product!;
		}

		public async Task<List<Product>> GetProducts(CancellationToken cancellationToken = default)
		{
			string uri = $"/catalog/get_products";
			List<Product>? response = await _httpClient.GetFromJsonAsync<List<Product>>(uri, cancellationToken);
			return response!;
		}

		public async Task AddProduct(Product product, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(product);

			if (product is null)
			{
				throw new ArgumentNullException(nameof(product));
			}

			var uri = $"/catalog/add_product";
			using var response = await _httpClient.PostAsJsonAsync(uri, product, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

		public async Task UpdateProduct(Product product, Guid id, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(product);

			if (product is null)
			{
				throw new ArgumentNullException(nameof(product));
			}

			var uri = $"/catalog/update_product?productId={id}";
			using var response = await _httpClient.PostAsJsonAsync(uri, product, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

		public async Task DeleteProduct(Guid id, CancellationToken cancellationToken = default)
		{
			var uri = $"/catalog/delete_product?productId={id}";
			using var response = await _httpClient.PostAsync(uri, null, cancellationToken);
			response.EnsureSuccessStatusCode();
		}

        public async Task Register(Account account, CancellationToken cancellationToken)
        {
			ArgumentNullException.ThrowIfNull(account);

			var uri = "/account/register";
			using var response = await _httpClient.PostAsJsonAsync(uri, account, cancellationToken);
			response.EnsureSuccessStatusCode();
        }

        public void Dispose()
		{
			((IDisposable)_httpClient).Dispose();
		}

        
    }
}
