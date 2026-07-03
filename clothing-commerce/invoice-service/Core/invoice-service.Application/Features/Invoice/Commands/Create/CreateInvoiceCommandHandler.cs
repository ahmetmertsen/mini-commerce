using FluentValidation;
using invoice_service.Application.Repositories;
using invoice_service.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invoice_service.Application.Features.Invoice.Commands.Create
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, CreateInvoiceCommandResponse>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IValidator<CreateInvoiceCommand> _validator;

        public CreateInvoiceCommandHandler(IInvoiceRepository invoiceRepository, IValidator<CreateInvoiceCommand> validator)
        {
            _invoiceRepository = invoiceRepository;
            _validator = validator;
        }

        public async Task<CreateInvoiceCommandResponse> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var invoice = new Domain.Entities.Invoice(request.OrderId, request.CustomerId, request.TotalAmount);

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();

            return new CreateInvoiceCommandResponse
            {
                Succeeded = true,
                Message = "Fatura başarıyla oluşturuldu.",
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber
            };
        }
    }
}
