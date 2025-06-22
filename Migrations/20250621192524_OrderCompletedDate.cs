using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderCompletedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompletedDate",
                table: "ServiceOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "ServiceOrders");
        }
    }
}
