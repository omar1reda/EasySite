using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWatched",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWatched",
                table: "Orders");
        }
    }
}
