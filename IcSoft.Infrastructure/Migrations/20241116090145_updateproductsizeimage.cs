using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateproductsizeimage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductSizeImage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductSizeImage",
                table: "Products");
        }
    }
}
