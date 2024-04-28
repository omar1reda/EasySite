using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SiteId",
                table: "Orders",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sites_SiteId",
                table: "Orders",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sites_SiteId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SiteId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Orders");
        }
    }
}
