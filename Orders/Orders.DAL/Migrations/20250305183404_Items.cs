using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Items : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_Orders_Order_Id",
                table: "OrderItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_Order_Id",
                table: "OrderItem");

            migrationBuilder.RenameTable(
                name: "OrderItem",
                newName: "Items");

            migrationBuilder.AlterColumn<long>(
                name: "Order_Id",
                table: "Items",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Order_Id1",
                table: "Items",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "OrderItem_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Order_Id1",
                table: "Items",
                column: "Order_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_Order_Id1",
                table: "Items",
                column: "Order_Id1",
                principalTable: "Orders",
                principalColumn: "Order_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_Order_Id1",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_Order_Id1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Order_Id1",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "OrderItem");

            migrationBuilder.AlterColumn<long>(
                name: "Order_Id",
                table: "OrderItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItem",
                table: "OrderItem",
                column: "OrderItem_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_Order_Id",
                table: "OrderItem",
                column: "Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_Orders_Order_Id",
                table: "OrderItem",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id");
        }
    }
}
