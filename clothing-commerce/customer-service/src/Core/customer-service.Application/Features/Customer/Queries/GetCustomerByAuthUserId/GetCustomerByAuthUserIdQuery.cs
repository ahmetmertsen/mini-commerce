using customer_service.Application.Dtos;
using MediatR;

namespace customer_service.Application.Features.Customer.Queries.GetCustomerByAuthUserId
{
    public class GetCustomerByAuthUserIdQuery : IRequest<CustomerDto>
    {
        public Guid AuthUserId { get; set; }
    }
}
