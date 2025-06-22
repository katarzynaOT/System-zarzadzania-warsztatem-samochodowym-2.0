using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkshopManager.Data.Migrations
{
    /// <inheritdoc />
    public partial class PartsUsedParts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ServiceOrders_OrderId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTask_ServiceOrders_OrderId",
                table: "ServiceTask");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedPart_ServiceTask_ServiceTaskId",
                table: "UsedPart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedPart",
                table: "UsedPart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceTask",
                table: "ServiceTask");

            migrationBuilder.DropColumn(
                name: "Part",
                table: "UsedPart");

            migrationBuilder.RenameTable(
                name: "UsedPart",
                newName: "UsedParts");

            migrationBuilder.RenameTable(
                name: "ServiceTask",
                newName: "ServiceTasks");

            migrationBuilder.RenameIndex(
                name: "IX_UsedPart_ServiceTaskId",
                table: "UsedParts",
                newName: "IX_UsedParts_ServiceTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceTask_OrderId",
                table: "ServiceTasks",
                newName: "IX_ServiceTasks_OrderId");

            /*
            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "ServiceOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);
            */
            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTaskId",
                table: "UsedParts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartId",
                table: "UsedParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCost",
                table: "UsedParts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "ServiceTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedParts",
                table: "UsedParts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceTasks",
                table: "ServiceTasks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_CarId",
                table: "ServiceOrders",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedParts_PartId",
                table: "UsedParts",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ServiceOrders_OrderId",
                table: "Comments",
                column: "OrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceOrders_Cars_CarId",
                table: "ServiceOrders",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTasks_ServiceOrders_OrderId",
                table: "ServiceTasks",
                column: "OrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsedParts_ServiceTasks_ServiceTaskId",
                table: "UsedParts",
                column: "ServiceTaskId",
                principalTable: "ServiceTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ServiceOrders_OrderId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceOrders_Cars_CarId",
                table: "ServiceOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTasks_ServiceOrders_OrderId",
                table: "ServiceTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_Parts_PartId",
                table: "UsedParts");

            migrationBuilder.DropForeignKey(
                name: "FK_UsedParts_ServiceTasks_ServiceTaskId",
                table: "UsedParts");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_ServiceOrders_CarId",
                table: "ServiceOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsedParts",
                table: "UsedParts");

            migrationBuilder.DropIndex(
                name: "IX_UsedParts_PartId",
                table: "UsedParts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceTasks",
                table: "ServiceTasks");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "ServiceOrders");

            migrationBuilder.DropColumn(
                name: "PartId",
                table: "UsedParts");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "UsedParts");

            migrationBuilder.RenameTable(
                name: "UsedParts",
                newName: "UsedPart");

            migrationBuilder.RenameTable(
                name: "ServiceTasks",
                newName: "ServiceTask");

            migrationBuilder.RenameIndex(
                name: "IX_UsedParts_ServiceTaskId",
                table: "UsedPart",
                newName: "IX_UsedPart_ServiceTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceTasks_OrderId",
                table: "ServiceTask",
                newName: "IX_ServiceTask_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "Comments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceTaskId",
                table: "UsedPart",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Part",
                table: "UsedPart",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "OrderId",
                table: "ServiceTask",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsedPart",
                table: "UsedPart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceTask",
                table: "ServiceTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ServiceOrders_OrderId",
                table: "Comments",
                column: "OrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTask_ServiceOrders_OrderId",
                table: "ServiceTask",
                column: "OrderId",
                principalTable: "ServiceOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsedPart_ServiceTask_ServiceTaskId",
                table: "UsedPart",
                column: "ServiceTaskId",
                principalTable: "ServiceTask",
                principalColumn: "Id");
        }
    }
}
