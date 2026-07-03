using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace auth_service.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_3_auth_outbox_dispatcher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AuthOutboxes",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "CorrelationId",
                table: "AuthOutboxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsSensitive",
                table: "AuthOutboxes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LastError",
                table: "AuthOutboxes",
                type: "nvarchar(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockedBy",
                table: "AuthOutboxes",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockedUntil",
                table: "AuthOutboxes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxRetryCount",
                table: "AuthOutboxes",
                type: "int",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextAttemptAt",
                table: "AuthOutboxes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "AuthOutboxes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AuthOutboxes",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Pending");

            migrationBuilder.Sql("""
                UPDATE AuthOutboxes
                   SET CorrelationId = TRY_CONVERT(uniqueidentifier, JSON_VALUE(Payload, '$.CorrelationId')),
                       IsSensitive = CASE
                           WHEN JSON_VALUE(Payload, '$.IsSensitive') IN ('true', 'True', '1') THEN CAST(1 AS bit)
                           ELSE CAST(0 AS bit)
                       END
                 WHERE ISJSON(Payload) = 1
                   AND TRY_CONVERT(uniqueidentifier, JSON_VALUE(Payload, '$.CorrelationId')) IS NOT NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_AuthOutboxes_CorrelationId",
                table: "AuthOutboxes",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthOutboxes_LockedUntil",
                table: "AuthOutboxes",
                column: "LockedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_AuthOutboxes_ProcessedDate",
                table: "AuthOutboxes",
                column: "ProcessedDate");

            migrationBuilder.CreateIndex(
                name: "IX_AuthOutboxes_Status_NextAttemptAt_OccuredOn",
                table: "AuthOutboxes",
                columns: new[] { "Status", "NextAttemptAt", "OccuredOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuthOutboxes_CorrelationId",
                table: "AuthOutboxes");

            migrationBuilder.DropIndex(
                name: "IX_AuthOutboxes_LockedUntil",
                table: "AuthOutboxes");

            migrationBuilder.DropIndex(
                name: "IX_AuthOutboxes_ProcessedDate",
                table: "AuthOutboxes");

            migrationBuilder.DropIndex(
                name: "IX_AuthOutboxes_Status_NextAttemptAt_OccuredOn",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "IsSensitive",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "LastError",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "LockedBy",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "LockedUntil",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "MaxRetryCount",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "NextAttemptAt",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "AuthOutboxes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AuthOutboxes");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "AuthOutboxes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
