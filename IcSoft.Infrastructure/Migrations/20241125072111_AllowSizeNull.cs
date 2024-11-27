using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AllowSizeNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Thêm cột "Color" có kiểu dữ liệu chuỗi, cho phép null
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "OrderItems",
                type: "nvarchar(max)",  // "nvarchar(max)" cho kiểu chuỗi trong SQL Server
                nullable: true);  // Cho phép null

            // Thêm cột "Size" có kiểu dữ liệu chuỗi, cho phép null
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "OrderItems",
                type: "nvarchar(max)",  // "nvarchar(max)" cho kiểu chuỗi trong SQL Server
                nullable: true);  // Cho phép null
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Xóa cột "Color"
            migrationBuilder.DropColumn(
                name: "Color",
                table: "OrderItems");

            // Xóa cột "Size"
            migrationBuilder.DropColumn(
                name: "Size",
                table: "OrderItems");
        }
    }
}
