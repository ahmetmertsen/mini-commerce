using notification_service.Application.Abstractions.Services;
using Shared.Messages.Notification.Enums;

namespace notification_service.Application.Services
{
    public class NotificationTemplateRenderer : INotificationTemplateRenderer
    {
        public string Render(string template, IReadOnlyDictionary<string, string> templateData)
        {
            var data = ExpandTemplateAliases(templateData);
            var rendered = template;

            foreach (var item in data)
            {
                rendered = rendered.Replace($"{{{item.Key}}}", item.Value);
            }

            return rendered;
        }

        public string ResolveTemplateName(NotificationType type)
        {
            return type switch
            {
                NotificationType.EmailVerification => "MAIL_VERIFY",
                NotificationType.PasswordReset => "FORGOT_PASSWORD",
                NotificationType.EmailChangeOld => "CHANGE_EMAIL_OLD",
                NotificationType.EmailChangeNew => "CHANGE_EMAIL",
                _ => type.ToString().ToUpperInvariant()
            };
        }

        public string ResolveSubject(NotificationType type)
        {
            return type switch
            {
                NotificationType.EmailVerification => "E-Posta Dogrulama",
                NotificationType.PasswordReset => "Sifre Sifirlama Talebi",
                NotificationType.EmailChangeOld => "E-Posta Degisikligi Onayi",
                NotificationType.EmailChangeNew => "Yeni E-Posta Dogrulama",
                NotificationType.OrderCreated => "Siparisiniz Alindi",
                NotificationType.OrderShipped => "Siparisiniz Kargoya Verildi",
                NotificationType.PaymentSucceeded => "Odemeniz Alindi",
                NotificationType.PaymentFailed => "Odeme Islemi Basarisiz",
                NotificationType.CampaignDiscount => "Yeni Kampanya",
                NotificationType.StockBackIn => "Urun Yeniden Stokta",
                NotificationType.Welcome => "Mini Commerce'e Hos Geldiniz",
                _ => "Mini Commerce Bildirimi"
            };
        }

        private static Dictionary<string, string> ExpandTemplateAliases(IReadOnlyDictionary<string, string> templateData)
        {
            var data = templateData.ToDictionary(item => item.Key, item => item.Value);
            if (data.TryGetValue("verification_code", out var verificationCode))
            {
                data.TryAdd("reset_code", verificationCode);
                data.TryAdd("reset_link", verificationCode);
                data.TryAdd("verify_code", verificationCode);
                data.TryAdd("verify_link", verificationCode);
                data.TryAdd("confirm_code", verificationCode);
                data.TryAdd("confirm_link", verificationCode);
            }

            return data;
        }
    }
}
