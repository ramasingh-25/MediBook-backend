using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediBook.PaymentService.Migrations
{
    /// <inheritdoc />
    public partial class AddProviderIdToPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProviderId",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Payments");
        }
    }
}
