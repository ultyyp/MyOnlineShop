using Microsoft.AspNetCore.Components;
using OnlineShopFrontend.HttpApiClient.Entities;
using OnlineShopFrontend.HttpApiClient;
using MudBlazor;
using OnlineShopFrontend.Dialogs;

namespace OnlineShopFrontend.Pages
{

    public partial class ProductInfo : IDisposable
    {
        [Inject]
        IMyShopClient? MyShopClient { get; set; }

        [Inject]
        IDialogService? DialogService { get; set; }

        [Inject]
        NavigationManager? NavigationManager { get; set; }

        [Parameter]
        public Guid ProductId { get; set; }

        [Parameter]
        public string? OpenedFrom { get; set; }

		private CancellationTokenSource _cts = new();

		private Product? _product { get; set; }

		protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _product = await MyShopClient!.GetProduct(ProductId, _cts.Token);
        }

        private void OpenEditDialog()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            DialogParameters dialogParameters = new()
            {
                { "ProductId", ProductId }
            };

            Action<IDialogReference, DialogResult> dialogCloseRequestedDelegate = (dialogReference, dialogResult) =>
            {
                StateHasChanged();
            };
            DialogService.OnDialogCloseRequested += dialogCloseRequestedDelegate;

            DialogService.Show<EditProductDialog>("Edit Product", dialogParameters, options);
            
        }

        private async Task DeleteProduct()
        {
            await MyShopClient!.DeleteProduct(ProductId, _cts.Token);
            ReturnToCatalog();
        }

		private void ReturnToCatalog()
        {
			NavigationManager!.NavigateTo($"/catalog");
		}

		public void Dispose()
		{
			_cts.Cancel();
		}
	}
}