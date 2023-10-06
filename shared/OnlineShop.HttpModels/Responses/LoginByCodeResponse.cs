using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Responses
{
    public record LoginByCodeResponse(Guid Id, string Name, string Token);
}
