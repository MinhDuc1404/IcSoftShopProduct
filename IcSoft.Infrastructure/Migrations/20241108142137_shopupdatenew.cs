using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class shopupdatenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductSale",
                table: "Products",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSale",
                table: "Products");
        }
    }
}
