using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediBook.AvailabilityService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailabilitySlots",
                columns: table => new
                {
                    SlotId = table.Column<string>(type: "text", nullable: false),
                    ProviderId = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    IsBooked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Recurrence = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilitySlots", x => x.SlotId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySlots_IsBlocked",
                table: "AvailabilitySlots",
                column: "IsBlocked");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySlots_IsBooked",
                table: "AvailabilitySlots",
                column: "IsBooked");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySlots_ProviderId",
                table: "AvailabilitySlots",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilitySlots_ProviderId_Date",
                table: "AvailabilitySlots",
                columns: new[] { "ProviderId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailabilitySlots");
        }
    }
}
