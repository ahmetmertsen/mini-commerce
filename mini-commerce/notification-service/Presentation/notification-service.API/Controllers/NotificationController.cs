using MediatR;
using Microsoft.AspNetCore.Mvc;
using notification_service.Application.Features.Notifications.Commands.DeleteNotification;
using notification_service.Application.Features.Notifications.Commands.MarkAllNotificationsAsRead;
using notification_service.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using notification_service.Application.Features.Notifications.Queries.GetNotificationsByUserId;
using notification_service.Application.Features.Notifications.Queries.GetUnreadNotificationCount;

namespace notification_service.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId, [FromQuery] int page = 1, [FromQuery] int size = 20, [FromQuery] bool onlyUnread = false, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetNotificationsByUserIdQuery
            {
                UserId = userId,
                Page = page,
                Size = size,
                OnlyUnread = onlyUnread
            }, cancellationToken);

            return Ok(response);
        }

        [HttpGet("users/{userId}/unread-count")]
        public async Task<IActionResult> GetUnreadCount(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new GetUnreadNotificationCountQuery
            {
                UserId = userId
            }, cancellationToken);

            return Ok(response);
        }

        [HttpPatch("{notificationId}/users/{userId}/read")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            var marked = await _mediator.Send(new MarkNotificationAsReadCommand
            {
                NotificationId = notificationId,
                UserId = userId
            }, cancellationToken);

            if (!marked)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPatch("users/{userId}/read-all")]
        public async Task<IActionResult> MarkAllAsRead(Guid userId, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new MarkAllNotificationsAsReadCommand
            {
                UserId = userId
            }, cancellationToken);

            return Ok(response);
        }

        [HttpDelete("{notificationId}/users/{userId}")]
        public async Task<IActionResult> Delete(Guid notificationId, Guid userId, CancellationToken cancellationToken)
        {
            var deleted = await _mediator.Send(new DeleteNotificationCommand
            {
                NotificationId = notificationId,
                UserId = userId
            }, cancellationToken);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
