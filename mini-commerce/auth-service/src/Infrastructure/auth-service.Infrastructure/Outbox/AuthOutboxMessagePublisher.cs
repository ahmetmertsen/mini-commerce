using System.Text.Json;
using auth_service.Application.Abstractions.Services;
using auth_service.Domain.Entities;
using MassTransit;
using Shared.Events.AuthEvents;
using Shared.Settings;

namespace auth_service.Infrastructure.Outbox
{
    public class AuthOutboxMessagePublisher : IAuthOutboxMessagePublisher
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly ISendEndpointProvider _sendEndpointProvider;

        public AuthOutboxMessagePublisher(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task PublishAsync(AuthOutbox outboxMessage, CancellationToken cancellationToken)
        {
            var messageType = AuthOutboxMessageTypeResolver.Resolve(outboxMessage.Type);
            var message = JsonSerializer.Deserialize(outboxMessage.Payload, messageType, SerializerOptions)
                ?? throw new InvalidOperationException($"Auth outbox payload could not be deserialized. Type: {outboxMessage.Type}");

            switch (message)
            {
                case NotificationRequested notificationRequested:
                    var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.NotificationRequestedQueue}"));
                    await endpoint.Send(notificationRequested, cancellationToken);
                    break;

                default:
                    throw new InvalidOperationException($"Auth outbox message type is resolved but not publishable. Type: {outboxMessage.Type}");
            }
        }
    }
}
