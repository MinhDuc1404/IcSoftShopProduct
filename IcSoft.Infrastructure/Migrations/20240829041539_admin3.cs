using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class admin3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6437f30-37e2-49f9-aa04-63bbf286b64b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee43b820-eabb-4af2-9454-36468bafebd6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "55be481e-4dbb-4305-be2b-13b24a26ab23", null, "admin", "admin" },
                    { "d0b0a8e4-ca5c-424d-999d-ad0e4bd749bf", null, "user", "user" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55be481e-4dbb-4305-be2b-13b24a26ab23");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0b0a8e4-ca5c-424d-999d-ad0e4bd749bf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "b6437f30-37e2-49f9-aa04-63bbf286b64b", null, "admin", "admin" },
                    { "ee43b820-eabb-4af2-9454-36468bafebd6", null, "user", "user" }
                });
        }
    }
}
