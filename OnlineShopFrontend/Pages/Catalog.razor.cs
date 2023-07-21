using Microsoft.AspNetCore.Components;
using OnlineShopFrontend.HttpApiClient.Entities;
using OnlineShopFrontend.HttpApiClient;

namespace OnlineShopFrontend.Pages
{
    public partial class Catalog : IDisposable
    {
		[Inject]
		IMyShopClient? MyShopClient { get; set; }

		[Inject]
		NavigationManager? NavigationManager { get; set; }

		private List<Product>? _products;

        private CancellationTokenSource _cts = new();

        protected override async Task OnInitializedAsync()
        {
            _products = await MyShopClient!.GetProducts(_cts.Token);
        }

        private Task OpenProductById(Product product)
        {
            NavigationManager!.NavigateTo($"/productinfo/{product.Id}");
            return Task.CompletedTask;
        }

        private Task OpenAddProduct()
        {
			NavigationManager!.NavigateTo($"/addproduct");
			return Task.CompletedTask;
        }

		public void Dispose()
		{
			_cts.Cancel();
		}
	}
}