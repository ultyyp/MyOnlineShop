using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Interfaces
{
	public interface IApplicationPasswordHasher
	{
		string HashPassword(string password);
		bool VerifyHashedPassword(string hashedPassword, string providedPassword, out bool rehashNeeded);
	}
}
