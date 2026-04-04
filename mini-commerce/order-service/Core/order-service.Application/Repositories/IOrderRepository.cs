using order_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace order_service.Application.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task AddAsync(Order order);
        void Update(Order order);
        Task SaveChangesAsync();
    }
}
