using AutoMapper;
using customer_service.Application.Dtos;
using customer_service.Application.Exceptions;
using customer_service.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace customer_service.Application.Features.Customer.Queries.GetCustomerByAuthUserId
{
    public class GetCustomerByAuthUserIdQueryHandler : IRequestHandler<GetCustomerByAuthUserIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCustomerByAuthUserIdQueryHandler> _logger;

        public GetCustomerByAuthUserIdQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetCustomerByAuthUserIdQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CustomerDto> Handle(GetCustomerByAuthUserIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByAuthUserIdAsync(request.AuthUserId, cancellationToken);
            if (customer is null)
            {
                _logger.LogWarning("Customer not found by auth user id. AuthUserId: {AuthUserId}", request.AuthUserId);
                throw new NotFoundException("Auth kullanıcısına bağlı müşteri bulunamadı.");
            }

            _logger.LogInformation(
                "Customer resolved by auth user id. AuthUserId: {AuthUserId}, CustomerId: {CustomerId}",
                request.AuthUserId,
                customer.Id);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}
