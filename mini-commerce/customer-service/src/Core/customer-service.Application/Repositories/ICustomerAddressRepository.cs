using customer_service.Domain.Entities;

namespace customer_service.Application.Repositories
{
    public interface ICustomerAddressRepository
    {
        Task<IReadOnlyList<CustomerAddress>> GetByCustomerIdAsync(Guid customerId, bool includeDeleted, CancellationToken cancellationToken);
        Task AddAsync(CustomerAddress address, CancellationToken cancellationToken);
        Task ClearDefaultShippingAsync(Guid customerId, CancellationToken cancellationToken);
        Task ClearDefaultBillingAsync(Guid customerId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
