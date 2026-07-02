using AutoMapper;
using customer_service.Application.Dtos;
using customer_service.Application.Exceptions;
using customer_service.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using CustomerAddressEntity = customer_service.Domain.Entities.CustomerAddress;

namespace customer_service.Application.Features.Customer.Commands.AddCustomerAddress
{
    public class AddCustomerAddressCommandHandler : IRequestHandler<AddCustomerAddressCommand, AddCustomerAddressCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddCustomerAddressCommandHandler> _logger;

        public AddCustomerAddressCommandHandler(ICustomerRepository customerRepository, ICustomerAddressRepository customerAddressRepository, IMapper mapper, ILogger<AddCustomerAddressCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _customerAddressRepository = customerAddressRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AddCustomerAddressCommandResponse> Handle(AddCustomerAddressCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer is null)
            {
                _logger.LogWarning("Address add failed because customer was not found. CustomerId: {CustomerId}", request.CustomerId);
                throw new NotFoundException("Müşteri bulunamadı.");
            }

            if (request.IsDefaultShipping)
            {
                await _customerAddressRepository.ClearDefaultShippingAsync(request.CustomerId, cancellationToken);
                _logger.LogInformation("Default shipping address cleared. CustomerId: {CustomerId}", request.CustomerId);
            }

            if (request.IsDefaultBilling)
            {
                await _customerAddressRepository.ClearDefaultBillingAsync(request.CustomerId, cancellationToken);
                _logger.LogInformation("Default billing address cleared. CustomerId: {CustomerId}", request.CustomerId);
            }

            var address = CustomerAddressEntity.Create(
                request.CustomerId,
                request.Type,
                request.Title,
                request.RecipientFullName,
                request.PhoneNumber,
                request.City,
                request.District,
                request.AddressLine,
                request.PostalCode,
                request.Country,
                request.IsDefaultShipping,
                request.IsDefaultBilling);

            await _customerAddressRepository.AddAsync(address, cancellationToken);
            await _customerAddressRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "Customer address added. CustomerId: {CustomerId}, AddressId: {AddressId}, AddressType: {AddressType}, IsDefaultShipping: {IsDefaultShipping}, IsDefaultBilling: {IsDefaultBilling}",
                request.CustomerId,
                address.Id,
                address.Type,
                address.IsDefaultShipping,
                address.IsDefaultBilling);

            return new AddCustomerAddressCommandResponse
            {
                Succeeded = true,
                Message = "Müşteri adresi eklendi.",
                AddressId = address.Id,
                Address = _mapper.Map<CustomerAddressDto>(address)
            };
        }
    }
}
