using customer_service.Application.Repositories;
using customer_service.Domain.Entities;
using customer_service.Domain.Enums;
using customer_service.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace customer_service.Persistence.Repositories
{
    public class CustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly CustomerServiceDbContext _context;

        public CustomerAddressRepository(CustomerServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CustomerAddress>> GetByCustomerIdAsync(Guid customerId, bool includeDeleted, CancellationToken cancellationToken)
        {
            var query = _context.CustomerAddresses.Where(address => address.CustomerId == customerId);

            if (!includeDeleted)
            {
                query = query.Where(address => address.Status != AddressStatus.Deleted);
            }

            return await query
                .OrderByDescending(address => address.IsDefaultShipping)
                .ThenByDescending(address => address.IsDefaultBilling)
                .ThenByDescending(address => address.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(CustomerAddress address, CancellationToken cancellationToken)
        {
            await _context.CustomerAddresses.AddAsync(address, cancellationToken);
        }

        public async Task ClearDefaultShippingAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var defaultAddresses = await _context.CustomerAddresses
                .Where(address =>
                    address.CustomerId == customerId &&
                    address.Status == AddressStatus.Active &&
                    address.IsDefaultShipping)
                .ToListAsync(cancellationToken);

            foreach (var address in defaultAddresses)
            {
                address.IsDefaultShipping = false;
                address.UpdatedDate = DateTime.UtcNow;
            }
        }

        public async Task ClearDefaultBillingAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var defaultAddresses = await _context.CustomerAddresses
                .Where(address =>
                    address.CustomerId == customerId &&
                    address.Status == AddressStatus.Active &&
                    address.IsDefaultBilling)
                .ToListAsync(cancellationToken);

            foreach (var address in defaultAddresses)
            {
                address.IsDefaultBilling = false;
                address.UpdatedDate = DateTime.UtcNow;
            }
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
