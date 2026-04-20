using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Commands.Create
{
    public class CreateInvoiceCommandResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public Guid InvoiceId { get; set; }
        public string InvoiceNumber { get; set; } = null!;
    }
}
