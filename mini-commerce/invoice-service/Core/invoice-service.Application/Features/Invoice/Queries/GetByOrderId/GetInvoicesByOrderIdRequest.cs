using invoice_service.Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Queries.GetByOrderId
{
    public class GetInvoicesByOrderIdRequest : IRequest<List<InvoiceDto>>
    {
        public Guid OrderId { get; set; }
    }
}
