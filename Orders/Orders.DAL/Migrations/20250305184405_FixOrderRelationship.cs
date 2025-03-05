using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Orders.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_Order_Id1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_Order_Id1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Order_Id1",
                table: "Items");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Order_Id",
                table: "Items",
                column: "Order_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_Order_Id",
                table: "Items",
                column: "Order_Id",
                principalTable: "Orders",
                principalColumn: "Order_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_Order_Id",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_Order_Id",
                table: "Items");

            migrationBuilder.AddColumn<long>(
                name: "Order_Id1",
                table: "Items",
                type: "bigint",
                nullable: true);

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
    }
}
