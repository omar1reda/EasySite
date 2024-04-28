using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "AppUser",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "AppUser");
        }
    }
}
