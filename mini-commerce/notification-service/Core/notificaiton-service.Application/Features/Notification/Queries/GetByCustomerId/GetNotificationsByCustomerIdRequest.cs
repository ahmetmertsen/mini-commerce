using MediatR;
using notification_service.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace notificaiton_service.Application.Features.Notification.Queries.GetByCustomerId
{
    public class GetNotificationsByCustomerIdRequest : IRequest<List<NotificationDto>>
    {
        public Guid CustomerId { get; set; }
    }
}
