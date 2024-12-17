using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IcSoft.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcreateat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Sizes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Colors",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Colors");
        }
    }
}
