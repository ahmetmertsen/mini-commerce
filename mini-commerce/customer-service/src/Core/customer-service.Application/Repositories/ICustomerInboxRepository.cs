using customer_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Application.Repositories
{
    public interface ICustomerInboxRepository
    {
        Task<CustomerInbox?> GetByMessageIdAsync(Guid messageId, CancellationToken cancellationToken);
        Task AddAsync(CustomerInbox inboxMessage, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
