using System.Collections.Concurrent;

namespace OnlineShop.Domain.Interfaces
{
	public interface IPageRequestCounterService
	{
		ConcurrentDictionary<string, int> GetRequests();
		void AddOrIncrementRequest(string domain);
	}
}
