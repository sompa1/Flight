using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repules.Dal.Migrations
{
    public partial class init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "FlightId",
                table: "GPSRecords",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6d801a95-fdc7-4f51-a1ca-08d74a8d3191"),
                column: "ConcurrencyStamp",
                value: "435045a3-57f0-40e3-b31e-9cd42566baef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191"),
                column: "ConcurrencyStamp",
                value: "f53c133f-da48-4e62-9999-f353e4701958");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "FlightId",
                table: "GPSRecords",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6d801a95-fdc7-4f51-a1ca-08d74a8d3191"),
                column: "ConcurrencyStamp",
                value: "f4920f00-1f11-40d2-905a-0e35733285c6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191"),
                column: "ConcurrencyStamp",
                value: "b08ada16-4ce1-46e0-903a-dd5e0bb4ed5c");
        }
    }
}
