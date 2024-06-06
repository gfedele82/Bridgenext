using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bridgenext.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Document_History",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Size",
                table: "Document",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("679bd613-da71-48b9-bf5c-b7b598935b77"),
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTime(2024, 6, 5, 22, 47, 47, 393, DateTimeKind.Utc).AddTicks(2583), new DateTime(2024, 6, 5, 22, 47, 47, 393, DateTimeKind.Utc).AddTicks(2593) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Document_History");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Document");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("679bd613-da71-48b9-bf5c-b7b598935b77"),
                columns: new[] { "CreateDate", "ModifyDate" },
                values: new object[] { new DateTime(2024, 5, 31, 0, 0, 38, 988, DateTimeKind.Utc).AddTicks(4254), new DateTime(2024, 5, 31, 0, 0, 38, 988, DateTimeKind.Utc).AddTicks(4269) });
        }
    }
}
