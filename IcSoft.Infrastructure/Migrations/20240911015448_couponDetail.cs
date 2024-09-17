using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class couponDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "30e0eb31-4246-4f09-8b7e-859c345c386b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6aaeac8-7913-438f-a7d3-1a94e3282589");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03016250-9e7e-4f7c-b2c2-8dd2a30dadac", null, "user", "USER" },
                    { "4d81bb2d-b151-40f2-81e3-3a7715a79b5e", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03016250-9e7e-4f7c-b2c2-8dd2a30dadac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d81bb2d-b151-40f2-81e3-3a7715a79b5e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30e0eb31-4246-4f09-8b7e-859c345c386b", null, "admin", "ADMIN" },
                    { "b6aaeac8-7913-438f-a7d3-1a94e3282589", null, "user", "USER" }
                });
        }
    }
}
