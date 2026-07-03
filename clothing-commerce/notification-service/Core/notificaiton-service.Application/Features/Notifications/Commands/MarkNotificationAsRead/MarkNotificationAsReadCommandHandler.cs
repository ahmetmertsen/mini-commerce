using MediatR;
using notification_service.Application.Repositories;

namespace notification_service.Application.Features.Notifications.Commands.MarkNotificationAsRead
{
    public class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public MarkNotificationAsReadCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            var marked = await _notificationRepository.MarkAsReadAsync(request.NotificationId, request.UserId, cancellationToken);
            if (marked)
            {
                await _notificationRepository.SaveChangesAsync(cancellationToken);
            }

            return marked;
        }
    }
}
