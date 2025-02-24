using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cart.DAL.Migrations
{
    /// <inheritdoc />
    public partial class TotalCartPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Items",
                newName: "Price");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Carts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Carts");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Items",
                newName: "TotalPrice");
        }
    }
}
