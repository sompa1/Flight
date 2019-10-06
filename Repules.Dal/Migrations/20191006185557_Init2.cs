using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repules.Dal.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserRoles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId", "Discriminator" },
                values: new object[] { new Guid("bc9ecb96-a585-4c27-98b7-5ddad62cae63"), new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191"), "ApplicationUserRole" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("bc9ecb96-a585-4c27-98b7-5ddad62cae63"), new Guid("a973cb5e-a900-4417-a1cb-08d74a8d3191") });

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserRoles");
        }
    }
}
