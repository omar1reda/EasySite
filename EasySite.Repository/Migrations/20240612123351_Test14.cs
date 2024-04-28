using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Test14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "AppUser");

            migrationBuilder.AddColumn<bool>(
                name: "PaymentMade",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "AmountDue",
                table: "AppUser",
                type: "float",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "YourAmount",
                table: "AppUser",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMade",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "YourAmount",
                table: "AppUser");

            migrationBuilder.AlterColumn<float>(
                name: "AmountDue",
                table: "AppUser",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AmountPaid",
                table: "AppUser",
                type: "real",
                nullable: true);
        }
    }
}
