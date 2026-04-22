using FluentValidation;
using MediatR;
using notification_service.Application.Dtos;
using notification_service.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Features.Notification.Queries.GetByCustomerId
{
    public class GetNotificationsByCustomerIdRequestHandler : IRequestHandler<GetNotificationsByCustomerIdRequest, List<NotificationDto>>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IValidator<GetNotificationsByCustomerIdRequest> _validator;

        public GetNotificationsByCustomerIdRequestHandler(INotificationRepository notificationRepository, IValidator<GetNotificationsByCustomerIdRequest> validator)
        {
            _notificationRepository = notificationRepository;
            _validator = validator;
        }

        public async Task<List<NotificationDto>> Handle(GetNotificationsByCustomerIdRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var notifications = await _notificationRepository.GetByCustomerIdAsync(request.CustomerId);

            return notifications.Select(notification => new NotificationDto
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
            }).ToList();
        }
    }
}
