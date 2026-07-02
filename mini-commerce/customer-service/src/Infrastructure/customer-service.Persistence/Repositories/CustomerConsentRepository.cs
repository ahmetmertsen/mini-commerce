using customer_service.Application.Repositories;
using customer_service.Domain.Entities;
using customer_service.Domain.Enums;
using customer_service.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace customer_service.Persistence.Repositories
{
    public class CustomerConsentRepository : ICustomerConsentRepository
    {
        private readonly CustomerServiceDbContext _context;

        public CustomerConsentRepository(CustomerServiceDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CustomerConsent>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _context.CustomerConsents
                .Where(consent => consent.CustomerId == customerId)
                .OrderByDescending(consent => consent.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public Task<CustomerConsent?> GetActiveByTypeAsync(Guid customerId, ConsentType type, CancellationToken cancellationToken)
        {
            return _context.CustomerConsents.FirstOrDefaultAsync(consent =>
                consent.CustomerId == customerId &&
                consent.Type == type &&
                consent.Status == ConsentStatus.Granted,
                cancellationToken);
        }

        public async Task AddAsync(CustomerConsent consent, CancellationToken cancellationToken)
        {
            await _context.CustomerConsents.AddAsync(consent, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
