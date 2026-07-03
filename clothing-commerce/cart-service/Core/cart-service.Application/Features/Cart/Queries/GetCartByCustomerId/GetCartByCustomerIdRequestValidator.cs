using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cart_service.Application.Features.Cart.Queries.GetCartByCustomerId
{
    public class GetCartByCustomerIdRequestValidator : AbstractValidator<GetCartByCustomerIdRequest>
    {
        public GetCartByCustomerIdRequestValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Müşteri numarası boş olamaz.");
        }
    }
}
