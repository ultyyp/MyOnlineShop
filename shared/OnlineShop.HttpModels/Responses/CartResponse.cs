using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.HttpModels.Responses
{
    public class CartResponse
    {
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }

    public record CartItemDTO(Guid Id, string ProductName, double Quantity);
}
