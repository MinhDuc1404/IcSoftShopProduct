using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modelDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "509a45d2-386a-434d-8476-ce00043cfd56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "860ca1b9-25ff-4c29-b2a3-99478b481fde");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "86838840-3eab-4aea-8773-fbf2aa603a4f", null, "user", "USER" },
                    { "bd17ed9e-8fe8-4a6c-9fa0-7cf7df3cc7fe", null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "86838840-3eab-4aea-8773-fbf2aa603a4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bd17ed9e-8fe8-4a6c-9fa0-7cf7df3cc7fe");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "509a45d2-386a-434d-8476-ce00043cfd56", null, "admin", "ADMIN" },
                    { "860ca1b9-25ff-4c29-b2a3-99478b481fde", null, "user", "USER" }
                });
        }
    }
}
