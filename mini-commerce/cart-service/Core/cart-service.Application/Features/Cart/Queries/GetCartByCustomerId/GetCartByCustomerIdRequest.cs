using cart_service.Application.Dtos;
using cart_service.Application.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Features.Cart.Queries.GetCartByCustomerId
{
    public class GetCartByCustomerIdRequest : IRequest<CartDto>
    {
        public Guid CustomerId { get; set; }
    }
}
