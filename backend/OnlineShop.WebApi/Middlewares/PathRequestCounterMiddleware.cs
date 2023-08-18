using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OnlineShop.Domain.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OnlineShop.WebApi.Middlewares
{
	public class PathRequestCounterMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<PathRequestCounterMiddleware> _logger;
		private readonly IPageRequestCounterService _counterService;

		public PathRequestCounterMiddleware(RequestDelegate next,
			ILogger<PathRequestCounterMiddleware> logger,
			IPageRequestCounterService counterService)
		{
			_next = next ?? throw new ArgumentNullException(nameof(next));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_counterService = counterService ?? throw new ArgumentNullException(nameof(counterService));
		}

		public async Task InvokeAsync(HttpContext context)
		{
			string domain = context.Request.Path;
			
			_counterService.DomainRequestCounterDictionary.AddOrUpdate(domain, 1, (_, existing) => existing + 1);

			//_logger.LogInformation($"Logging Domain Dictionary:");
			//foreach (var kvp in _counterService.DomainRequestCounterDictionary)
			//{
			//	_logger.LogInformation($"{kvp.Key}: {kvp.Value}");
			//}
			//_logger.LogInformation($"Finished Logging Domain Dictionary.");

			await _next(context);
		}
	}
}
