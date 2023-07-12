using Microsoft.AspNetCore.Components;
using OnlineShopFrontend.Interfaces;
using OnlineShopFrontend.Entities;

namespace OnlineShopFrontend.Pages
{
    public partial class Cart
    {
		[Inject]
		NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }   
    }
}