using OnlineShop.Domain.Entities;
using OnlineShop.Domain.Interfaces;
using System.Collections.Concurrent;

namespace OnlineShop.Domain.Services
{
	public class PageRequestCounterService : IPageRequestCounterService
	{
		private readonly ConcurrentDictionary<string, int> _domainRequestCounterDictionary = new ConcurrentDictionary<string, int>();

		public IReadOnlyList<PageCounter> GetRequests()
		{
			var list = new List<PageCounter>(_domainRequestCounterDictionary.Select(kvp => new PageCounter(kvp.Key, kvp.Value)));
			return list;
		}

		public Task AddOrIncrementRequest(string domain)
		{
			_domainRequestCounterDictionary.AddOrUpdate(domain, 1, (_, existing) => existing + 1);
			return Task.CompletedTask;
		}
	}
}
