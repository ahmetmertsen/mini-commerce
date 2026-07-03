using FluentValidation;
using invoice_service.Application.Dtos;
using invoice_service.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Queries.GetByOrderId
{
    public class GetInvoicesByOrderIdRequestHandler : IRequestHandler<GetInvoicesByOrderIdRequest, List<InvoiceDto>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IValidator<GetInvoicesByOrderIdRequest> _validator;

        public GetInvoicesByOrderIdRequestHandler(IInvoiceRepository invoiceRepository, IValidator<GetInvoicesByOrderIdRequest> validator)
        {
            _invoiceRepository = invoiceRepository;
            _validator = validator;
        }

        public async Task<List<InvoiceDto>> Handle(GetInvoicesByOrderIdRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var invoices = await _invoiceRepository.GetByOrderIdAsync(request.OrderId);

            return invoices.Select(invoice => new InvoiceDto
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                CustomerId = invoice.CustomerId,
                InvoiceNumber = invoice.InvoiceNumber,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status,
                CreatedDate = invoice.CreatedDate
            }).ToList();
        }
    }
}
