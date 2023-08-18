using OnlineShop.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Services
{
	public class PageRequestCounterService : IPageRequestCounterService
	{
		public ConcurrentDictionary<string, int> DomainRequestCounterDictionary { get; } = new ConcurrentDictionary<string, int>();
	}
}
