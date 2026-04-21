using Microsoft.EntityFrameworkCore;
using shipment_service.Application.Repositories;
using shipment_service.Domain.Entities;
using shipment_service.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Persistence.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly ShipmentServiceDbContext _context;

        public ShipmentRepository(ShipmentServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Shipment?> GetByIdAsync(Guid id) => await _context.Shipments
            .FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(Shipment shipment) => await _context.Shipments.AddAsync(shipment);
        
        public void Update(Shipment shipment) => _context.Shipments.Update(shipment);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<List<Shipment>> GetByOrderIdAsync(Guid orderId) => await _context.Shipments
            .Where(s => s.OrderId == orderId)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync();

        public async Task<List<Shipment>> GetByCustomerIdAsync(Guid customerId) => await _context.Shipments
            .Where(s => s.CustomerId == customerId)
            .OrderByDescending(s => s.CreatedDate)
            .ToListAsync();

        public async Task<Shipment?> GetByTrackingNumberAsync(string trackingNumber) => await _context.Shipments
            .FirstOrDefaultAsync(x => x.TrackingNumber == trackingNumber);
    }
}
