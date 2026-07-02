using MediatR;
using notification_service.Application.Repositories;

namespace notification_service.Application.Features.Notifications.Queries.GetNotificationsByUserId
{
    public class GetNotificationsByUserIdQueryHandler : IRequestHandler<GetNotificationsByUserIdQuery, GetNotificationsByUserIdQueryResponse>
    {
        private readonly INotificationRepository _notificationRepository;

        public GetNotificationsByUserIdQueryHandler(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<GetNotificationsByUserIdQueryResponse> Handle(GetNotificationsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var page = Math.Max(1, request.Page);
            var size = Math.Clamp(request.Size, 1, 100);
            var result = await _notificationRepository.GetPagedByUserIdAsync(request.UserId, page, size, request.OnlyUnread, cancellationToken);

            return new GetNotificationsByUserIdQueryResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount,
                Page = page,
                Size = size
            };
        }
    }
}
