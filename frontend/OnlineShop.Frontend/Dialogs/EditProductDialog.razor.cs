using OnlineShop.HttpApiClient.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OnlineShop.HttpApiClient;

namespace OnlineShop.Frontend.Dialogs
{
    public partial class EditProductDialog
    {
        [Inject]
        IMyShopClient MyShopClient { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public Guid ProductId { get; set; }

        private CancellationTokenSource _cts = new();
        private string? Name { get; set; }
        private decimal Price { get; set; }

        async Task Submit()
        {
            if (!(string.IsNullOrEmpty(Name) || Price == 0))
            {
                var product = new Product(Name, Price);
                product.Id = ProductId;
                await MyShopClient.UpdateProduct(product, ProductId, _cts.Token);
                MudDialog.Close(DialogResult.Ok(true));
                NavigationManager.NavigateTo("/catalog");
            }
            else
            {
                MudDialog.Close(DialogResult.Ok(true));
                NavigationManager.NavigateTo("/catalog");
            }
        }

        void Cancel() => MudDialog.Cancel();
    }
}