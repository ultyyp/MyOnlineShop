using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using OnlineShop.HttpApiClient;

namespace OnlineShop.Frontend.Pages
{
	public abstract class AppComponentBase : ComponentBase
	{
		[Inject] protected IMyShopClient Client { get; private set; }
		[Inject] protected ILocalStorageService LocalStorage { get; private set; }

		protected override async Task OnInitializedAsync()
		{
			await base.OnInitializedAsync();

			if (Client.IsAuthorizationTokenSet) return;


			var token = await LocalStorage.GetItemAsync<string>("token");

			if (!string.IsNullOrEmpty(token))
			{
				Client.SetAuthorizationToken(token);
			}
		}
	}
}
