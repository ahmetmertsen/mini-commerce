using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Commands.Create
{
    public class CreateInvoiceCommand : IRequest<CreateInvoiceCommandResponse>
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
