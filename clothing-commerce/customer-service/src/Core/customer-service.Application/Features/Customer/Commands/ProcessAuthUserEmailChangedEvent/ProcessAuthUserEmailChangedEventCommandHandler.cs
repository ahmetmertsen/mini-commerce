using System.Text.Json;
using customer_service.Application.Repositories;
using customer_service.Domain.Entities;
using customer_service.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace customer_service.Application.Features.Customer.Commands.ProcessAuthUserEmailChangedEvent
{
    public class ProcessAuthUserEmailChangedEventCommandHandler : IRequestHandler<ProcessAuthUserEmailChangedEventCommand, ProcessAuthUserEmailChangedEventCommandResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerInboxRepository _customerInboxRepository;
        private readonly ILogger<ProcessAuthUserEmailChangedEventCommandHandler> _logger;

        public ProcessAuthUserEmailChangedEventCommandHandler(
            ICustomerRepository customerRepository,
            ICustomerInboxRepository customerInboxRepository,
            ILogger<ProcessAuthUserEmailChangedEventCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _customerInboxRepository = customerInboxRepository;
            _logger = logger;
        }

        public async Task<ProcessAuthUserEmailChangedEventCommandResponse> Handle(ProcessAuthUserEmailChangedEventCommand request, CancellationToken cancellationToken)
        {
            var response = new ProcessAuthUserEmailChangedEventCommandResponse
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
                var error = $"Customer not found for auth user id: {request.AuthUserId}";
                MarkFailed(inboxMessage, error);
                await _customerRepository.SaveChangesAsync(cancellationToken);

                response.Succeeded = false;
                response.Error = error;

                _logger.LogWarning(
                    "Auth user email changed event could not be processed because customer was not found. MessageId: {MessageId}, CorrelationId: {CorrelationId}, AuthUserId: {AuthUserId}",
                    request.MessageId,
                    request.CorrelationId,
                    request.AuthUserId);

                return response;
            }

            if (!string.Equals(customer.Email, request.NewEmail, StringComparison.OrdinalIgnoreCase))
            {
                customer.UpdateEmail(request.NewEmail);
            }

            MarkProcessed(inboxMessage);
            await _customerRepository.SaveChangesAsync(cancellationToken);

            response.CustomerId = customer.Id;
            response.Succeeded = true;

            _logger.LogInformation(
                "Auth user email changed event processed. MessageId: {MessageId}, CorrelationId: {CorrelationId}, AuthUserId: {AuthUserId}, CustomerId: {CustomerId}",
                request.MessageId,
                request.CorrelationId,
                request.AuthUserId,
                customer.Id);

            return response;
        }

        private static CustomerInbox CreateInboxMessage(ProcessAuthUserEmailChangedEventCommand request)
        {
            return new CustomerInbox
            {
                MessageId = request.MessageId,
                CorrelationId = request.CorrelationId,
                MessageType = CustomerInboxMessageType.AuthUserEmailChangedEvent,
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

        private static void MarkFailed(CustomerInbox inboxMessage, string error)
        {
            inboxMessage.Status = InboxMessageStatus.Failed;
            inboxMessage.RetryCount += 1;
            inboxMessage.FailedAt = DateTime.UtcNow;
            inboxMessage.Error = error.Length <= 2048 ? error : error[..2048];
        }
    }
}
