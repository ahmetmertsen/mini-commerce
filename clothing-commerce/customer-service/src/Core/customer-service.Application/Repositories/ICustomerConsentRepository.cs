using customer_service.Domain.Entities;
using customer_service.Domain.Enums;

namespace customer_service.Application.Repositories
{
    public interface ICustomerConsentRepository
    {
        Task<IReadOnlyList<CustomerConsent>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<CustomerConsent?> GetActiveByTypeAsync(Guid customerId, ConsentType type, CancellationToken cancellationToken);
        Task AddAsync(CustomerConsent consent, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
