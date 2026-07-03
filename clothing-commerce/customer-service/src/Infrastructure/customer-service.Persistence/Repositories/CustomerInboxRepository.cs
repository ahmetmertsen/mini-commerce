using customer_service.Application.Repositories;
using customer_service.Domain.Entities;
using customer_service.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Persistence.Repositories
{
    public class CustomerInboxRepository : ICustomerInboxRepository
    {
        private readonly CustomerServiceDbContext _context;

        public CustomerInboxRepository(CustomerServiceDbContext context)
        {
            _context = context;
        }

        public Task<CustomerInbox?> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken)
        {
            return _context.CustomerInboxes.FirstOrDefaultAsync(inbox => inbox.MessageId == messageId, cancellationToken);
        }

        public async Task AddAsync(CustomerInbox inboxMessage, CancellationToken cancellationToken)
        {
            await _context.CustomerInboxes.AddAsync(inboxMessage, cancellationToken);
        }

        public Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
