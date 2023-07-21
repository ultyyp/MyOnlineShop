using Microsoft.AspNetCore.Components;
using OnlineShopFrontend.HttpApiClient.Entities;
using OnlineShopFrontend.HttpApiClient;

namespace OnlineShopFrontend.Pages
{
    public partial class AddProduct : IDisposable
    {
		[Inject]
		IMyShopClient? MyShopClient { get; set; }

		[Inject]
		NavigationManager? NavigationManager { get; set; }

        private CancellationTokenSource _cts = new();

		private string? Name { get; set; }
        private decimal Price { get; set; }

		public void Dispose()
		{
            _cts.Cancel();
		}

		private async Task AddProductToCatalog()
        {
            var Product = new Product(Name!, Price);
            await MyShopClient!.AddProduct(Product, _cts.Token);
            NavigationManager!.NavigateTo("/catalog");
        }

        private Task BackToCatalog()
        {
            NavigationManager!.NavigateTo("/catalog");
            return Task.CompletedTask;
        }
    }
}