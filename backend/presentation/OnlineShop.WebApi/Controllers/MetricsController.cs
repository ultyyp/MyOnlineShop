using Microsoft.AspNetCore.Mvc;
using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using OnlineShop.HttpModels.Responses;
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
		public IReadOnlyList<PageCounterResponse> GetDomainRequestCount()
		{
			var requests = _counterService.GetRequests();
			var list = new List<PageCounterResponse>(requests.Select(req => new PageCounterResponse(req.Domain, req.Count)));
			return list;
		}
	}
}
