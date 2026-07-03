using MediatR;

namespace notification_service.Application.Features.Notifications.Queries.GetUnreadNotificationCount
{
    public class GetUnreadNotificationCountQuery : IRequest<GetUnreadNotificationCountQueryResponse>
    {
        public Guid UserId { get; set; }
    }

    public class GetUnreadNotificationCountQueryResponse
    {
        public int Count { get; set; }
    }
}
