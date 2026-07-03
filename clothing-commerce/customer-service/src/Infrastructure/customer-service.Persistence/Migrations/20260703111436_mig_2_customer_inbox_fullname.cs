using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace customer_service.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig_2_customer_inbox_fullname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Customers",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE Customers
                   SET FullName = LTRIM(RTRIM(CONCAT(FirstName, ' ', LastName)))
                """);

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customers");

            migrationBuilder.CreateTable(
                name: "CustomerInboxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInboxes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInboxes_MessageId",
                table: "CustomerInboxes",
                column: "MessageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInboxes_Status_ReceivedAt",
                table: "CustomerInboxes",
                columns: new[] { "Status", "ReceivedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerInboxes");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("""
                UPDATE Customers
                   SET FirstName = FullName,
                       LastName = ''
                """);

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Customers");
        }
    }
}
