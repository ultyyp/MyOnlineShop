using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
	public interface IPageRequestCounterService
	{
		ConcurrentDictionary<string, int> DomainRequestCounterDictionary { get; }
	}
}
