using FluentValidation;
using invoice_service.Application.Dtos;
using invoice_service.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Queries.GetById
{
    public class GetInvoiceByIdRequestHandler : IRequestHandler<GetInvoiceByIdRequest, InvoiceDto>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IValidator<GetInvoiceByIdRequest> _validator;

        public GetInvoiceByIdRequestHandler(IInvoiceRepository invoiceRepository, IValidator<GetInvoiceByIdRequest> validator)
        {
            _invoiceRepository = invoiceRepository;
            _validator = validator;
        }

        public async Task<InvoiceDto> Handle(GetInvoiceByIdRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var invoice = await _invoiceRepository.GetByIdAsync(request.Id);
            if (invoice == null)
            {
                throw new Exception("Fatura bulunamadı.");
            }

            return new InvoiceDto
            {
                Id = invoice.Id,
                OrderId = invoice.OrderId,
                CustomerId = invoice.CustomerId,
                InvoiceNumber = invoice.InvoiceNumber,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status,
                CreatedDate = invoice.CreatedDate
            };
        }
    }
}
