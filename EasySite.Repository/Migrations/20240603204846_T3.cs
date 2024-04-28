using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class T3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SittingFormOrder_Country_CountryId",
                table: "SittingFormOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_SittingFormOrder_Email_EmailId",
                table: "SittingFormOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_SittingFormOrder_FullName_FullNameId",
                table: "SittingFormOrder");

            migrationBuilder.DropColumn(
                name: "Text_Important",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "IsImportant",
                table: "Address",
                newName: "IsRequired");

            migrationBuilder.RenameColumn(
                name: "IsAddress",
                table: "Address",
                newName: "Activation");

            migrationBuilder.AlterColumn<int>(
                name: "FullNameId",
                table: "SittingFormOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EmailId",
                table: "SittingFormOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "SittingFormOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Phone",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Government",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "FullName",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Email",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "TextPlaceholder",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SittingFormOrder_Country_CountryId",
                table: "SittingFormOrder",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SittingFormOrder_Email_EmailId",
                table: "SittingFormOrder",
                column: "EmailId",
                principalTable: "Email",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SittingFormOrder_FullName_FullNameId",
                table: "SittingFormOrder",
                column: "FullNameId",
                principalTable: "FullName",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SittingFormOrder_Country_CountryId",
                table: "SittingFormOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_SittingFormOrder_Email_EmailId",
                table: "SittingFormOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_SittingFormOrder_FullName_FullNameId",
                table: "SittingFormOrder");

            migrationBuilder.DropColumn(
                name: "TextPlaceholder",
                table: "Address");

            migrationBuilder.RenameColumn(
                name: "IsRequired",
                table: "Address",
                newName: "IsImportant");

            migrationBuilder.RenameColumn(
                name: "Activation",
                table: "Address",
                newName: "IsAddress");

            migrationBuilder.AlterColumn<int>(
                name: "FullNameId",
                table: "SittingFormOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmailId",
                table: "SittingFormOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "SittingFormOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Phone",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Government",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "FullName",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Email",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TextPlaceholder",
                table: "Country",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text_Important",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_SittingFormOrder_Country_CountryId",
                table: "SittingFormOrder",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SittingFormOrder_Email_EmailId",
                table: "SittingFormOrder",
                column: "EmailId",
                principalTable: "Email",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SittingFormOrder_FullName_FullNameId",
                table: "SittingFormOrder",
                column: "FullNameId",
                principalTable: "FullName",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
