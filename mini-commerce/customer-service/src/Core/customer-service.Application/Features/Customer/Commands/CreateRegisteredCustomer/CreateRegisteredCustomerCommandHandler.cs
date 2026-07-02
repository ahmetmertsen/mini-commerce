using AutoMapper;
using customer_service.Application.Dtos;
using customer_service.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using CustomerEntity = customer_service.Domain.Entities.Customer;

namespace customer_service.Application.Features.Customer.Commands.CreateRegisteredCustomer
{
    public class CreateRegisteredCustomerCommandHandler : IRequestHandler<CreateRegisteredCustomerCommand, CreateRegisteredCustomerCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRegisteredCustomerCommandHandler> _logger;

        public CreateRegisteredCustomerCommandHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<CreateRegisteredCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateRegisteredCustomerCommandResponse> Handle(CreateRegisteredCustomerCommand request, CancellationToken cancellationToken)
        {
            var existingCustomer = await _customerRepository.GetByAuthUserIdAsync(request.AuthUserId, cancellationToken);
            if (existingCustomer is not null)
            {
                _logger.LogInformation(
                    "Registered customer already exists. AuthUserId: {AuthUserId}, CustomerId: {CustomerId}",
                    request.AuthUserId,
                    existingCustomer.Id);

                return new CreateRegisteredCustomerCommandResponse
                {
                    Succeeded = true,
                    AlreadyExists = true,
                    Message = "Kayıtlı müşteri zaten mevcut.",
                    CustomerId = existingCustomer.Id,
                    CustomerNumber = existingCustomer.CustomerNumber,
                    Customer = _mapper.Map<CustomerDto>(existingCustomer)
                };
            }

            var customer = CustomerEntity.CreateRegistered(request.AuthUserId, request.FirstName, request.LastName, request.Email, request.PhoneNumber);

            await _customerRepository.AddAsync(customer, cancellationToken);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Registered customer created. AuthUserId: {AuthUserId}, CustomerId: {CustomerId}, CustomerNumber: {CustomerNumber}",
                request.AuthUserId,
                customer.Id,
                customer.CustomerNumber);

            return new CreateRegisteredCustomerCommandResponse
            {
                Succeeded = true,
                Message = "Kayıtlı müşteri oluşturuldu.",
                CustomerId = customer.Id,
                CustomerNumber = customer.CustomerNumber,
                Customer = _mapper.Map<CustomerDto>(customer)
            };
        }
    }
}
