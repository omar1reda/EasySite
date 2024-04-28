using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsUpSalle",
                table: "OrderItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsUpSalle",
                table: "OrderItems");
        }
    }
}
