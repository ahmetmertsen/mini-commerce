using customer_service.Application.Repositories;
using customer_service.Domain.Entities;
using customer_service.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace customer_service.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerServiceDbContext _context;

        public CustomerRepository(CustomerServiceDbContext context)
        {
            _context = context;
        }

        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return _context.Customers.FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
        }

        public Task<Customer?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken)
        {
            return CustomerWithDetails()
                .FirstOrDefaultAsync(customer => customer.Id == id, cancellationToken);
        }

        public Task<Customer?> GetByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken)
        {
            return CustomerWithDetails()
                .FirstOrDefaultAsync(customer => customer.AuthUserId == authUserId, cancellationToken);
        }

        public Task<bool> ExistsByAuthUserIdAsync(Guid authUserId, CancellationToken cancellationToken)
        {
            return _context.Customers.AnyAsync(customer => customer.AuthUserId == authUserId, cancellationToken);
        }

        public async Task AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        private IQueryable<Customer> CustomerWithDetails()
        {
            return _context.Customers
                .Include(customer => customer.Addresses)
                .Include(customer => customer.Consents);
        }
    }
}
