using MediatR;
using notification_service.Application.Repositories;

namespace notification_service.Application.Features.Notifications.Commands.DeleteNotification
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, bool>
    {
        private readonly INotificationRepository _notificationRepository;

        public DeleteNotificationCommandHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _notificationRepository.SoftDeleteByIdAndUserAsync(request.NotificationId, request.UserId, cancellationToken);
            if (deleted)
            {
                await _notificationRepository.SaveChangesAsync(cancellationToken);
            }

            return deleted;
        }
    }
}
