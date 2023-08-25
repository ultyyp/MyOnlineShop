using OnlineShop.Domain.Entities;
using System.Collections.Concurrent;

namespace OnlineShop.Domain.Interfaces
{
	public interface IPageRequestCounterService
	{
		IReadOnlyList<PageCounter> GetRequests();
		Task AddOrIncrementRequest(string domain);
	}
}
