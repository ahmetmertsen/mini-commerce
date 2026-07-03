using shipment_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shipment_service.Domain.Entities
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string TrackingNumber { get; set; } = null!;
        public string CarrierCompany { get; set; } = null!;
        public ShipmentStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        private Shipment() { }

        public Shipment(Guid orderId, Guid customerId, string carrierCompany)
        {
            if (orderId == Guid.Empty)
                throw new ArgumentException("Geçerli bir sipariş bilgisi girilmelidir.");

            if (customerId == Guid.Empty)
                throw new ArgumentException("Geçerli bir müşteri bilgisi girilmelidir.");

            if (string.IsNullOrWhiteSpace(carrierCompany))
                throw new ArgumentException("Kargo firması boş olamaz.");

            Id = Guid.NewGuid();
            OrderId = orderId;
            CustomerId = customerId;
            CarrierCompany = carrierCompany;
            TrackingNumber = GenerateTrackingNumber();
            Status = ShipmentStatus.Preparing;
            CreatedDate = DateTime.UtcNow;
        }

        private string GenerateTrackingNumber()
        {
            return $"TRK-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }
    }
}
