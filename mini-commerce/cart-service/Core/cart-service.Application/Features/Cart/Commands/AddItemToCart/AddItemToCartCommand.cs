using cart_service.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Features.Cart.Commands.AddItemToCart
{
    public class AddItemToCartCommand : IRequest<AddItemToCartCommandResponse>
    {
        public Guid CustomerId { get; set; }
        public AddCartItemDto CartItem { get; set; } = null!;
    }
}
