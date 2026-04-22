using FluentValidation;
using MediatR;
using notification_service.Application.Repositories;
using notification_service.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notification_service.Application.Features.Notification.Commands.Create
{
    public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, CreateNotificationCommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IValidator<CreateNotificationCommand> _validator;

        public CreateNotificationCommandHandler(INotificationRepository notificationRepository, IValidator<CreateNotificationCommand> validator)
        {
            _notificationRepository = notificationRepository;
            _validator = validator;
        }

        public async Task<CreateNotificationCommandResponse> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var notification = new Domain.Entities.Notification(request.CustomerId, request.OrderId, request.Type, request.Channel, request.Title, request.Message);
            if (request.SimulateSuccess)
            {
                notification.IsSent = true;
                notification.SentDate = DateTime.UtcNow;
            }
            else
            {
                notification.IsSent = false;
                notification.FailureReason = "Bildirim gönderilemedi.";
            }

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

            return new CreateNotificationCommandResponse
            {
                Succeeded = notification.IsSent,
                Message = notification.IsSent ? "Bildirim başarıyla oluşturuldu ve gönderildi." : "Bildirim oluşturuldu ancak gönderilemedi.",
                NotificationId = notification.Id
            };
        }
    }
}
