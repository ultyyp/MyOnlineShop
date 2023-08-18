using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Interfaces;
using System.Collections.Concurrent;

namespace OnlineShop.WebApi.Controllers
{
	[ApiController]
	[Route("metrics")]
	public class MetricsController : ControllerBase
	{
		private readonly IPageRequestCounterService _counterService;

		public MetricsController(IPageRequestCounterService counterService)
		{
			_counterService = counterService;
		}

		[HttpGet("get_pathcounter")]
		public ConcurrentDictionary<string, int> GetDomainRequestCount()
		{
			return _counterService.GetRequests();
		}
	}
}
