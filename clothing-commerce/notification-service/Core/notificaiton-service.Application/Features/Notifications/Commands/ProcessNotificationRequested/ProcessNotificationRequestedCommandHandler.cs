using MediatR;
using notification_service.Application.Abstractions.Services;
using notification_service.Application.Repositories;
using notification_service.Domain.Entities;
using notification_service.Domain.Enums;
using Shared.Events.AuthEvents;
using Shared.Messages.Notification.Enums;
using System.Text.Json;

namespace notification_service.Application.Features.Notifications.Commands.ProcessNotificationRequested
{
    public class ProcessNotificationRequestedCommandHandler : IRequestHandler<ProcessNotificationRequestedCommand, ProcessNotificationRequestedCommandResponse>
    {
        private const string SensitiveBodyPlaceholder = "[Sensitive notification content omitted]";

        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly INotificationInboxRepository _inboxRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMailTemplateRepository _mailTemplateRepository;
        private readonly IMailService _mailService;
        private readonly INotificationTemplateRenderer _templateRenderer;

        public ProcessNotificationRequestedCommandHandler(
            INotificationInboxRepository inboxRepository,
            INotificationRepository notificationRepository,
            IMailTemplateRepository mailTemplateRepository,
            IMailService mailService,
            INotificationTemplateRenderer templateRenderer)
        {
            _inboxRepository = inboxRepository;
            _notificationRepository = notificationRepository;
            _mailTemplateRepository = mailTemplateRepository;
            _mailService = mailService;
            _templateRenderer = templateRenderer;
        }

        public async Task<ProcessNotificationRequestedCommandResponse> Handle(ProcessNotificationRequestedCommand request, CancellationToken cancellationToken)
        {
            var message = request.Message;
            var response = new ProcessNotificationRequestedCommandResponse
            {
                MessageId = message.NotificationId,
                CorrelationId = message.CorrelationId
            };

            var inboxMessage = await _inboxRepository.GetByMessageIdAsync(message.NotificationId, cancellationToken);
            if (inboxMessage?.Status == InboxMessageStatus.Processed)
            {
                response.AlreadyProcessed = true;
                response.Succeeded = true;
                return response;
            }

            if (inboxMessage == null)
            {
                inboxMessage = new NotificationInbox
                {
                    Id = Guid.NewGuid(),
                    MessageId = message.NotificationId,
                    CorrelationId = message.CorrelationId,
                    Type = message.Type,
                    Channel = message.Channel,
                    Payload = CreateInboxPayload(message),
                    Status = InboxMessageStatus.Received,
                    ReceivedAt = DateTime.UtcNow
                };

                await _inboxRepository.AddAsync(inboxMessage, cancellationToken);
                await _inboxRepository.SaveChangesAsync(cancellationToken);
            }

            inboxMessage.MarkProcessing();

            var notification = await _notificationRepository.GetByMessageIdAsync(message.NotificationId, cancellationToken);
            if (notification == null)
            {
                notification = CreateNotification(message);
                await _notificationRepository.AddAsync(notification, cancellationToken);
            }

            try
            {
                await ProcessDeliveryAsync(message, notification, cancellationToken);

                var now = DateTime.UtcNow;
                notification.MarkSent(now);
                inboxMessage.MarkProcessed(now);

                await _notificationRepository.SaveChangesAsync(cancellationToken);

                response.Succeeded = true;
                return response;
            }
            catch (Exception exception)
            {
                var now = DateTime.UtcNow;
                var error = exception.Message;

                notification.MarkFailed(error, now);
                inboxMessage.MarkFailed(error, now);

                await _notificationRepository.SaveChangesAsync(cancellationToken);

                response.Succeeded = false;
                response.Error = error;
                return response;
            }
        }

        private async Task ProcessDeliveryAsync(NotificationRequested message, Notification notification, CancellationToken cancellationToken)
        {
            if (message.Channel != NotificationChannel.Email)
            {
                throw new InvalidOperationException($"Notification channel is not supported yet. Channel: {message.Channel}");
            }

            if (string.IsNullOrWhiteSpace(message.RecipientEmail))
            {
                throw new InvalidOperationException("Email notification requires RecipientEmail.");
            }

            var templateName = _templateRenderer.ResolveTemplateName(message.Type);
            var template = await _mailTemplateRepository.GetByNameAsync(templateName, cancellationToken)
                ?? throw new InvalidOperationException($"Mail template was not found. Name: {templateName}");

            var subject = _templateRenderer.ResolveSubject(message.Type);
            var renderedBody = _templateRenderer.Render(template.Value, message.TemplateData);

            notification.Subject = subject;
            notification.Body = message.IsSensitive ? SensitiveBodyPlaceholder : renderedBody;

            await _mailService.SendMailAsync(message.RecipientEmail, subject, renderedBody);
        }

        private Notification CreateNotification(NotificationRequested message)
        {
            return new Notification
            {
                Id = Guid.NewGuid(),
                MessageId = message.NotificationId,
                CorrelationId = message.CorrelationId,
                Type = message.Type,
                Channel = message.Channel,
                Status = NotificationStatus.Pending,
                UserId = message.RecipientUserId,
                RecipientEmail = message.RecipientEmail,
                RecipientPhone = message.RecipientPhone,
                Subject = _templateRenderer.ResolveSubject(message.Type),
                Body = message.IsSensitive ? SensitiveBodyPlaceholder : string.Empty,
                IsSensitive = message.IsSensitive,
                CreatedAt = DateTime.UtcNow,
                isActive = true,
                isDeleted = false
            };
        }

        private static string CreateInboxPayload(NotificationRequested message)
        {
            if (!message.IsSensitive)
            {
                return JsonSerializer.Serialize(message, SerializerOptions);
            }

            var sanitizedMessage = new NotificationRequested
            {
                NotificationId = message.NotificationId,
                RecipientUserId = message.RecipientUserId,
                RecipientEmail = message.RecipientEmail,
                RecipientPhone = message.RecipientPhone,
                Type = message.Type,
                Channel = message.Channel,
                TemplateData = message.TemplateData.ToDictionary(item => item.Key, _ => "***"),
                IsSensitive = message.IsSensitive,
                CorrelationId = message.CorrelationId,
                OccurredAt = message.OccurredAt
            };

            return JsonSerializer.Serialize(sanitizedMessage, SerializerOptions);
        }
    }
}
