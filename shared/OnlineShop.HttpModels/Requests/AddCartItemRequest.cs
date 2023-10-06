using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Requests
{
    public record AddCartItemRequest(Guid AccountId, Guid ProductId, double Quantity);
}
