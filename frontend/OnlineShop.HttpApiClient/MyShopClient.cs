using OnlineShop.HttpApiClient.Entities;
using OnlineShop.HttpModels.Requests;
using OnlineShop.HttpModels.Responses;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Json;

namespace OnlineShop.HttpApiClient
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

		public async Task Register(RegisterRequest request, CancellationToken cancellationToken)
		{
			var uri = "/account/register";
			try
			{
				await PostAsJsonAndDeserializeAsync<RegisterRequest, RegisterResponse>(uri, request, cancellationToken);
			}
			catch (MyShopApiException ex)
			{
				throw;
			}
		}

		public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancellationToken)
		{
			const string uri = "account/login";
			try
			{
				return await PostAsJsonAndDeserializeAsync<LoginRequest, LoginResponse>(uri, request, cancellationToken);
			}
			catch (MyShopApiException ex)
			{
				throw;
			}
		}

		public async Task<ConcurrentDictionary<string, int>> GetMetricsPathCounter(CancellationToken cancellationToken)
		{
			const string uri = "metrics/get_pathcounter";
			return await _httpClient.GetFromJsonAsync<ConcurrentDictionary<string, int>>(uri, cancellationToken);
		}

		public async Task<TResponse> PostAsJsonAndDeserializeAsync<TRequest, TResponse>(
		string uri,
		TRequest request,
		CancellationToken cancellationToken)
		{
			ArgumentNullException.ThrowIfNull(request);

			using var response = await _httpClient.PostAsJsonAsync(uri, request, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				if (response.StatusCode == HttpStatusCode.Conflict)
				{
					var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: cancellationToken);
					throw new MyShopApiException(error!);
				}
				else if (response.StatusCode == HttpStatusCode.BadRequest)
				{
					var details = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(cancellationToken: cancellationToken);
					throw new MyShopApiException(response.StatusCode, details!);
				}
				else
				{
					throw new MyShopApiException($"Unknown Error! {response.StatusCode}");
				}
			}

			
			var responseData = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
			return responseData!;
			
		}


		public void Dispose()
		{
			((IDisposable)_httpClient).Dispose();
		}

        
    }
}
