using invoice_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Dtos
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public InvoiceStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
