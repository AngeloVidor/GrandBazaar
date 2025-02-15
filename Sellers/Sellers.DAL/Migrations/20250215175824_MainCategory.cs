using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sellers.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MainCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainCategory",
                table: "Sellers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainCategory",
                table: "Sellers");
        }
    }
}
