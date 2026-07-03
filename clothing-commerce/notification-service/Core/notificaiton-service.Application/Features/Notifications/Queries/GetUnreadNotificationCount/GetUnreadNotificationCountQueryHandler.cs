using MediatR;
using notification_service.Application.Repositories;

namespace notification_service.Application.Features.Notifications.Queries.GetUnreadNotificationCount
{
    public class GetUnreadNotificationCountQueryHandler : IRequestHandler<GetUnreadNotificationCountQuery, GetUnreadNotificationCountQueryResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetUnreadNotificationCountQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<GetUnreadNotificationCountQueryResponse> Handle(GetUnreadNotificationCountQuery request, CancellationToken cancellationToken)
        {
            var count = await _notificationRepository.GetUnreadCountAsync(request.UserId, cancellationToken);

            return new GetUnreadNotificationCountQueryResponse
            {
                Count = count
            };
        }
    }
}
