using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Dtos
{
    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public Guid ProductVariantId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}
