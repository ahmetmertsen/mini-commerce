using shipment_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Application.Repositories
{
    public interface IShipmentRepository
    {
        Task<Shipment?> GetByIdAsync(Guid id);
        Task AddAsync(Shipment shipment);
        void Update(Shipment shipment);
        Task SaveChangesAsync();

        Task<List<Shipment>> GetByOrderIdAsync(Guid orderId);
    }
}
