using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repules.Dal.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6d801a95-fdc7-4f51-a1ca-08d74a8d3191"),
                column: "ConcurrencyStamp",
                value: "5d45ff63-8723-4732-a16d-550fd997e887");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191"),
                column: "ConcurrencyStamp",
                value: "e0544d58-6cb3-4f6b-b064-9062a8925f1f");
        }
    }
}
