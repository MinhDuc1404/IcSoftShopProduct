using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cd7427f-6f04-4def-a4bf-3fae5f533712");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b548c4e-bcc0-43c2-b123-1df3bd84dd64");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "30e0eb31-4246-4f09-8b7e-859c345c386b", null, "admin", "ADMIN" },
                    { "b6aaeac8-7913-438f-a7d3-1a94e3282589", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    { "2cd7427f-6f04-4def-a4bf-3fae5f533712", null, "admin", "ADMIN" },
                    { "8b548c4e-bcc0-43c2-b123-1df3bd84dd64", null, "user", "USER" }
                });
        }
    }
}
