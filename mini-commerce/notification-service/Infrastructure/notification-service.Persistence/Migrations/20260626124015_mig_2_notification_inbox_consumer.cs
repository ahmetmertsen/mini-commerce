using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace notification_service.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2_notification_inbox_consumer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "MailTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationInboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationInboxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RecipientEmail = table.Column<string>(type: "nvarchar(320)", maxLength: 320, nullable: true),
                    RecipientPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSensitive = table.Column<bool>(type: "bit", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailTemplates_Name",
                table: "MailTemplates",
                column: "Name",
                unique: true);

            migrationBuilder.InsertData(
                table: "MailTemplates",
                columns: new[] { "Id", "Name", "Value", "CreatedAt", "UpdateAt", "isActive", "isDeleted" },
                values: new object[,]
                {
                    {
                        new Guid("11111111-1111-1111-1111-111111111111"),
                        "MAIL_VERIFY",
                        "<div style=\"font-family: Arial, Helvetica, sans-serif; line-height:1.6; color:#111;\"><p style=\"margin:0 0 12px;\">Merhaba, {full_name}</p><p style=\"margin:0 0 16px;\">Hesabınız için <b>e-posta doğrulama</b> işlemini tamamlamanız gerekiyor. Aşağıdaki kodu doğrulama ekranına girerek e-posta adresinizi doğrulayabilirsiniz.</p><p style=\"margin:0 0 18px; font-size:28px; font-weight:bold; letter-spacing:4px; color:#1a73e8;\">{verification_code}</p><p style=\"margin:0 0 10px; font-size:14px; color:#444;\">Bu kod 10 dakika geçerlidir. Kodu kimseyle paylaşmayın.</p><hr style=\"border:none; border-top:1px solid #eee; margin:18px 0;\" /><p style=\"margin:0 0 6px; font-size:13px; color:#666;\">Bu talebi siz yapmadıysanız bu e-postayı görmezden gelebilirsiniz.</p><p style=\"margin:18px 0 0; font-size:12px; color:#999;\">© {app_name} — Otomatik e-posta, lütfen yanıtlamayın.</p></div>",
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        true,
                        false
                    },
                    {
                        new Guid("22222222-2222-2222-2222-222222222222"),
                        "FORGOT_PASSWORD",
                        "<div style=\"font-family: Arial, Helvetica, sans-serif; line-height:1.6; color:#111;\"><p style=\"margin:0 0 12px;\">Merhaba, {full_name}</p><p style=\"margin:0 0 16px;\">Hesabınız için bir <b>şifre sıfırlama</b> talebi aldık. Aşağıdaki kodu şifre sıfırlama ekranına girerek yeni şifrenizi belirleyebilirsiniz.</p><p style=\"margin:0 0 18px; font-size:28px; font-weight:bold; letter-spacing:4px; color:#1a73e8;\">{verification_code}</p><p style=\"margin:0 0 10px; font-size:14px; color:#444;\">Bu kod 10 dakika geçerlidir. Kodu kimseyle paylaşmayın.</p><hr style=\"border:none; border-top:1px solid #eee; margin:18px 0;\" /><p style=\"margin:0 0 6px; font-size:13px; color:#666;\">Bu talebi siz yapmadıysanız bu e-postayı görmezden gelebilirsiniz.</p><p style=\"margin:18px 0 0; font-size:12px; color:#999;\">© {app_name} — Otomatik e-posta, lütfen yanıtlamayın.</p></div>",
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        true,
                        false
                    },
                    {
                        new Guid("33333333-3333-3333-3333-333333333333"),
                        "CHANGE_EMAIL",
                        "<div style=\"font-family: Arial, Helvetica, sans-serif; line-height:1.6; color:#111;\"><p style=\"margin:0 0 12px;\">Merhaba, {full_name}</p><p style=\"margin:0 0 16px;\">Hesabınız için <b>e-posta adresi değiştirme</b> talebi aldık. Bu yeni e-posta adresini onaylamak için aşağıdaki kodu doğrulama ekranına girin.</p><p style=\"margin:0 0 18px; font-size:28px; font-weight:bold; letter-spacing:4px; color:#1a73e8;\">{verification_code}</p><p style=\"margin:0 0 10px; font-size:14px; color:#444;\">Bu kod 10 dakika geçerlidir. Kodu kimseyle paylaşmayın.</p><hr style=\"border:none; border-top:1px solid #eee; margin:18px 0;\" /><p style=\"margin:0 0 6px; font-size:13px; color:#666;\">Bu talebi siz yapmadıysanız bu e-postayı görmezden gelebilirsiniz.</p><p style=\"margin:18px 0 0; font-size:12px; color:#999;\">© {app_name} — Otomatik e-posta, lütfen yanıtlamayın.</p></div>",
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        true,
                        false
                    },
                    {
                        new Guid("44444444-4444-4444-4444-444444444444"),
                        "CHANGE_EMAIL_OLD",
                        "<div style=\"font-family: Arial, Helvetica, sans-serif; line-height:1.6; color:#111;\"><p style=\"margin:0 0 12px;\">Merhaba, {full_name}</p><p style=\"margin:0 0 16px;\">{app_name} hesabınızın e-posta adresi <b>{new_email}</b> olarak değiştirilmek isteniyor. Bu işlemi siz başlattıysanız aşağıdaki kodu doğrulama ekranına girin.</p><p style=\"margin:0 0 18px; font-size:28px; font-weight:bold; letter-spacing:4px; color:#1a73e8;\">{verification_code}</p><p style=\"margin:0 0 10px; font-size:14px; color:#444;\">Bu kod 10 dakika geçerlidir. Bu işlemi siz başlatmadıysanız kodu kimseyle paylaşmayın.</p><hr style=\"border:none; border-top:1px solid #eee; margin:18px 0;\" /><p style=\"margin:0 0 6px; font-size:13px; color:#666;\">Bu talebi siz yapmadıysanız hesabınızı güvene almanız önerilir.</p><p style=\"margin:18px 0 0; font-size:12px; color:#999;\">© {app_name} — Otomatik e-posta, lütfen yanıtlamayın.</p></div>",
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        new DateTime(2026, 6, 26, 0, 0, 0, DateTimeKind.Utc),
                        true,
                        false
                    }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationInboxes_CorrelationId",
                table: "NotificationInboxes",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationInboxes_MessageId",
                table: "NotificationInboxes",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificationInboxes_Status",
                table: "NotificationInboxes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CorrelationId",
                table: "Notifications",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MessageId",
                table: "Notifications",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailTemplates");

            migrationBuilder.DropTable(
                name: "NotificationInboxes");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CustomerId",
                table: "Notifications",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_OrderId",
                table: "Notifications",
                column: "OrderId");
        }
    }
}
