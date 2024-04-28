using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Tests2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerPermitions_AppUser_UserId",
                table: "ManagerPermitions");

            migrationBuilder.DropIndex(
                name: "IX_ManagerPermitions_UserId",
                table: "ManagerPermitions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ManagerPermitions");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "AppUser",
                newName: "MarketerId");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "ManagerPermitions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ManagerPermitionsId",
                table: "AppUser",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MangerId",
                table: "AppUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManagerPermitions_AppUserId",
                table: "ManagerPermitions",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerPermitions_AppUser_AppUserId",
                table: "ManagerPermitions",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManagerPermitions_AppUser_AppUserId",
                table: "ManagerPermitions");

            migrationBuilder.DropIndex(
                name: "IX_ManagerPermitions_AppUserId",
                table: "ManagerPermitions");

            migrationBuilder.DropColumn(
                name: "ManagerPermitionsId",
                table: "AppUser");

            migrationBuilder.DropColumn(
                name: "MangerId",
                table: "AppUser");

            migrationBuilder.RenameColumn(
                name: "MarketerId",
                table: "AppUser",
                newName: "AppUserId");

            migrationBuilder.AlterColumn<int>(
                name: "AppUserId",
                table: "ManagerPermitions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ManagerPermitions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerPermitions_UserId",
                table: "ManagerPermitions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ManagerPermitions_AppUser_UserId",
                table: "ManagerPermitions",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
