using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "Phone",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "Government",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "FullName",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "Email",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "Country",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "Address",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Phone",
                newName: "Activation");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Government",
                newName: "Activation");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "FullName",
                newName: "Activation");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Email",
                newName: "Activation");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Country",
                newName: "Activation");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Address",
                newName: "Activation");
        }
    }
}
