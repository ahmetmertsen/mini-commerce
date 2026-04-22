using notification_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Repositories
{
    public interface INotificatitonRepository
    {
        Task<Notification?> GetByIdAsync(Guid id);
        Task AddAsync(Notification notification);
        void Update(Notification notification);
        Task SaveChangesAsync();
        Task<List<Notification>> GetByCustomerIdAsync(Guid customerId);

    }
}
