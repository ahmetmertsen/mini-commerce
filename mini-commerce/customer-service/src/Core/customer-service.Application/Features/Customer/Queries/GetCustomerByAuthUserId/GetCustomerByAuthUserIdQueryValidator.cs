using FluentValidation;

namespace customer_service.Application.Features.Customer.Queries.GetCustomerByAuthUserId
{
    public class GetCustomerByAuthUserIdQueryValidator : AbstractValidator<GetCustomerByAuthUserIdQuery>
    {
        public GetCustomerByAuthUserIdQueryValidator()
        {
            RuleFor(query => query.AuthUserId)
                .NotEmpty().WithMessage("Auth user id boş olamaz.");
        }
    }
}
