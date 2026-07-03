using AutoMapper;
using customer_service.Application.Dtos;
using customer_service.Domain.Entities;

namespace customer_service.Application.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerAddress, CustomerAddressDto>();
            CreateMap<CustomerConsent, CustomerConsentDto>();
        }
    }
}
