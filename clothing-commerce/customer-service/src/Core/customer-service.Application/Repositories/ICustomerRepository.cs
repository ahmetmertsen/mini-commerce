using customer_service.Domain.Entities;

namespace customer_service.Application.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken);
        Task<Customer?> GetByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken);
        Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken);
        Task AddAsync(Customer customer, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
