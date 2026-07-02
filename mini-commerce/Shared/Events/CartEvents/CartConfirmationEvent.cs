using Shared.Messages.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.CartEvents
{
    public class CartConfirmationEvent
    {
        public Guid CorrelationId { get; set; }
        public Guid CartId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemMessage> CartItems { get; set; } = new();
    }
}
