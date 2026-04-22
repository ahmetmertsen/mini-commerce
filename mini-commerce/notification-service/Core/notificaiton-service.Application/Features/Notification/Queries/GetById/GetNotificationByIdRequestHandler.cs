using FluentValidation;
using MediatR;
using notification_service.Application.Dtos;
using notification_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Features.Notification.Queries.GetById
{
    public class GetNotificationByIdRequestHandler : IRequestHandler<GetNotificationByIdRequest, NotificationDto>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IValidator<GetNotificationByIdRequest> _validator;

        public GetNotificationByIdRequestHandler(INotificationRepository notificationRepository, IValidator<GetNotificationByIdRequest> validator)
        {
            _notificationRepository = notificationRepository;
            _validator = validator;
        }

        public async Task<NotificationDto> Handle(GetNotificationByIdRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var notification = await _notificationRepository.GetByIdAsync(request.Id);
            if (notification == null)
            {
                throw new Exception("Bildirim bulunamadı.");
            }

            return new NotificationDto
            {
                Id = notification.Id,
                CustomerId = notification.CustomerId,
                OrderId = notification.OrderId,
                Type = notification.Type,
                Channel = notification.Channel,
                Title = notification.Title,
                Message = notification.Message,
                IsSent = notification.IsSent,
                CreatedDate = notification.CreatedDate,
                SentDate = notification.SentDate,
                FailureReason = notification.FailureReason
            };
        }
    }
}
