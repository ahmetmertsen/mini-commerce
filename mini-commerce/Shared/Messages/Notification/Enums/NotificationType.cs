using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messages.Notification.Enums
{
    public enum NotificationType
    {
        EmailVerification = 1,
        PasswordReset,
        EmailChangeOld,
        EmailChangeNew,

        OrderCreated,
        OrderShipped,
        PaymentSucceeded,
        PaymentFailed,
        CampaignDiscount,
        StockBackIn,
        Welcome
    }
}
