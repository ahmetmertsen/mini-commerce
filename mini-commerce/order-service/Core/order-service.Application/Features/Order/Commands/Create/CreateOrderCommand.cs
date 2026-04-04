using order_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order_service.Application.Features.Order.Commands.Create
{
    public class CreateOrderCommand
    {
        public Guid CustomerId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
