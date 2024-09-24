using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a27d7af-f089-4c7e-858f-505fc51b8331");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce0f642b-f383-4b5b-bf22-5c4072ee5ba4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2716877c-f3e9-474e-a9ae-214b8d27a2de", null, "admin", "ADMIN" },
                    { "505a5ea1-5437-4835-ba41-3730365bfa18", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2716877c-f3e9-474e-a9ae-214b8d27a2de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "505a5ea1-5437-4835-ba41-3730365bfa18");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a27d7af-f089-4c7e-858f-505fc51b8331", null, "admin", "ADMIN" },
                    { "ce0f642b-f383-4b5b-bf22-5c4072ee5ba4", null, "user", "USER" }
                });
        }
    }
}
