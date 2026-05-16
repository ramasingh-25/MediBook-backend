using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediBook.ProviderService.Migrations
{
    /// <inheritdoc />
    public partial class AddFullNameToProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Providers",
                type: "text",
                nullable: false,
                defaultValue: "Unknown");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Providers");
        }
    }
}
