using OnlineShop.Domain.Interfaces;
using System.Collections.Concurrent;

namespace OnlineShop.Domain.Services
{
	public class PageRequestCounterService : IPageRequestCounterService
	{
		private ConcurrentDictionary<string, int> _domainRequestCounterDictionary = new ConcurrentDictionary<string, int>();

		public ConcurrentDictionary<string, int> GetRequests()
		{
			return _domainRequestCounterDictionary;
		}

		public void AddOrIncrementRequest(string domain)
		{
			_domainRequestCounterDictionary.AddOrUpdate(domain, 1, (_, existing) => existing + 1);
		}
	}
}
