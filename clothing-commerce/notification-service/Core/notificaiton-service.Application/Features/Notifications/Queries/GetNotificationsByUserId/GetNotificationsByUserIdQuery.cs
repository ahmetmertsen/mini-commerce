using MediatR;
using notification_service.Application.Dtos;

namespace notification_service.Application.Features.Notifications.Queries.GetNotificationsByUserId
{
    public class GetNotificationsByUserIdQuery : IRequest<GetNotificationsByUserIdQueryResponse>
    {
        public Guid UserId { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
        public bool OnlyUnread { get; set; }
    }

    public class GetNotificationsByUserIdQueryResponse
    {
        public List<NotificationDto> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}
