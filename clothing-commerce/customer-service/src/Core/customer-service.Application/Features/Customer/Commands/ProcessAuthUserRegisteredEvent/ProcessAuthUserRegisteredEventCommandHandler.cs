using System.Text.Json;
using customer_service.Application.Repositories;
using customer_service.Domain.Entities;
using customer_service.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using CustomerEntity = customer_service.Domain.Entities.Customer;

namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserRegisteredEvent
{
    public class ProcessAuthUserRegisteredEventCommandHandler : IRequestHandler<ProcessAuthUserRegisteredEventCommand, ProcessAuthUserRegisteredEventCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerInboxRepository _customerInboxRepository;
        private readonly ILogger<ProcessAuthUserRegisteredEventCommandHandler> _logger;

        public ProcessAuthUserRegisteredEventCommandHandler(
            ICustomerRepository customerRepository,
            ICustomerInboxRepository customerInboxRepository,
            ILogger<ProcessAuthUserRegisteredEventCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _customerInboxRepository = customerInboxRepository;
            _logger = logger;
        }

        public async Task<ProcessAuthUserRegisteredEventCommandResponse> Handle(ProcessAuthUserRegisteredEventCommand request, CancellationToken cancellationToken)
        {
            var response = new ProcessAuthUserRegisteredEventCommandResponse
            {
                MessageId = request.MessageId,
                CorrelationId = request.CorrelationId
            };

            var inboxMessage = await _customerInboxRepository.GetByMessageIdAsync(request.MessageId, cancellationToken);
            if (inboxMessage?.Status == InboxMessageStatus.Processed)
            {
                response.AlreadyProcessed = true;
                response.Succeeded = true;
                return response;
            }

            if (inboxMessage is null)
            {
                inboxMessage = CreateInboxMessage(request);
                await _customerInboxRepository.AddAsync(inboxMessage, cancellationToken);
            }

            MarkProcessing(inboxMessage);

            var customer = await _customerRepository.GetByAuthUserIdAsync(request.AuthUserId, cancellationToken);
            if (customer is null)
            {
                customer = CustomerEntity.CreateRegistered(request.AuthUserId, request.FullName, request.Email);
                await _customerRepository.AddAsync(customer, cancellationToken);
            }

            MarkProcessed(inboxMessage);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            response.CustomerId = customer.Id;
            response.Succeeded = true;

            _logger.LogInformation(
                "Auth user registered event processed. MessageId: {MessageId}, CorrelationId: {CorrelationId}, AuthUserId: {AuthUserId}, CustomerId: {CustomerId}",
                request.MessageId,
                request.CorrelationId,
                request.AuthUserId,
                customer.Id);

            return response;
        }

        private static CustomerInbox CreateInboxMessage(ProcessAuthUserRegisteredEventCommand request)
        {
            return new CustomerInbox
            {
                MessageId = request.MessageId,
                CorrelationId = request.CorrelationId,
                MessageType = CustomerInboxMessageType.AuthUserRegisteredEvent,
                Payload = JsonSerializer.Serialize(request),
                Status = InboxMessageStatus.Received,
                RetryCount = 0,
                ReceivedAt = DateTime.UtcNow
            };
        }

        private static void MarkProcessing(CustomerInbox inboxMessage)
        {
            inboxMessage.Status = InboxMessageStatus.Processing;
            inboxMessage.Error = null;
            inboxMessage.FailedAt = null;
        }

        private static void MarkProcessed(CustomerInbox inboxMessage)
        {
            inboxMessage.Status = InboxMessageStatus.Processed;
            inboxMessage.ProcessedAt = DateTime.UtcNow;
            inboxMessage.Error = null;
            inboxMessage.FailedAt = null;
        }
    }
}
