using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeItem",
                table: "Phone",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeItem",
                table: "Government",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeItem",
                table: "FullName",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeItem",
                table: "Email",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeItem",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TypeItem",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeItem",
                table: "Phone");

            migrationBuilder.DropColumn(
                name: "TypeItem",
                table: "Government");

            migrationBuilder.DropColumn(
                name: "TypeItem",
                table: "FullName");

            migrationBuilder.DropColumn(
                name: "TypeItem",
                table: "Email");

            migrationBuilder.DropColumn(
                name: "TypeItem",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "TypeItem",
                table: "Address");
        }
    }
}
