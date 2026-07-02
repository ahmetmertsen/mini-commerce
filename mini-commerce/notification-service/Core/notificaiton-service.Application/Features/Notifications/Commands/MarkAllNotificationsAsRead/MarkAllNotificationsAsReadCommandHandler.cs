using MediatR;
using notification_service.Application.Repositories;

namespace notification_service.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead
{
    public class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand, MarkAllNotificationsAsReadCommandResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkAllNotificationsAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<MarkAllNotificationsAsReadCommandResponse> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            var count = await _notificationRepository.MarkAllAsReadAsync(request.UserId, cancellationToken);
            await _notificationRepository.SaveChangesAsync(cancellationToken);

            return new MarkAllNotificationsAsReadCommandResponse
            {
                Count = count
            };
        }
    }
}
