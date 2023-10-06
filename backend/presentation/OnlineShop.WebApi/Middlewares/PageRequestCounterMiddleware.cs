using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OnlineShop.WebApi.Middlewares
{
	public class PageRequestCounterMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IPageRequestCounterService _counterService;

		public PageRequestCounterMiddleware(RequestDelegate next,
			IPageRequestCounterService counterService)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string domain = context.Request.Path;
			_counterService.AddOrIncrementRequest(domain);
			await _next(context);
		}
	}
}
