using customer_service.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace customer_service.Application.Dtos
{
    public class CustomerConsentDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public ConsentType Type { get; set; }
        public ConsentStatus Status { get; set; }
        public string? Version { get; set; }
        public string? Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? RevokedDate { get; set; }
    }
}
