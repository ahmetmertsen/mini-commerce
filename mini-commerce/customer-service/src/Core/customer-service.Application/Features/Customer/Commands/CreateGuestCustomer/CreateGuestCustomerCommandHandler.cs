using AutoMapper;
using customer_service.Application.Dtos;
using customer_service.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using CustomerEntity = customer_service.Domain.Entities.Customer;

namespace customer_service.Application.Features.Customer.Commands.CreateGuestCustomer
{
    public class CreateGuestCustomerCommandHandler : IRequestHandler<CreateGuestCustomerCommand, CreateGuestCustomerCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateGuestCustomerCommandHandler> _logger;

        public CreateGuestCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<CreateGuestCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateGuestCustomerCommandResponse> Handle(CreateGuestCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = CustomerEntity.CreateGuest(request.FirstName, request.LastName, request.Email, request.PhoneNumber);

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Guest customer created. CustomerId: {CustomerId}, CustomerNumber: {CustomerNumber}",
                customer.Id,
                customer.CustomerNumber);

            return new CreateGuestCustomerCommandResponse
            {
                Succeeded = true,
                Message = "Misafir müşteri oluşturuldu.",
                CustomerId = customer.Id,
                CustomerNumber = customer.CustomerNumber,
                Customer = _mapper.Map<CustomerDto>(customer)
            };
        }
    }
}
