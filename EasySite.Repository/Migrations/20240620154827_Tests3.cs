using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Tests3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HeaderCode",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlockFakeOrders",
                table: "Sites",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MassegeBlockFakeOrders",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeBlockFakeOrders",
                table: "Sites",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CrossSellingId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "IpAddres",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Utm_NameCampaign",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Utm_SourceCampaign",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CrossSelling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProductIdToRedirect = table.Column<int>(type: "int", nullable: false),
                    Sale = table.Column<int>(type: "int", nullable: false),
                    BottonTextAgree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BottonColorAgree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButtonTextReject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButtonColorReject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossSelling", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CrossSellingId",
                table: "Products",
                column: "CrossSellingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CrossSelling_CrossSellingId",
                table: "Products",
                column: "CrossSellingId",
                principalTable: "CrossSelling",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CrossSelling_CrossSellingId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "CrossSelling");

            migrationBuilder.DropIndex(
                name: "IX_Products_CrossSellingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HeaderCode",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "IsBlockFakeOrders",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "MassegeBlockFakeOrders",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "TimeBlockFakeOrders",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "CrossSellingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IpAddres",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Utm_NameCampaign",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Utm_SourceCampaign",
                table: "Orders");
        }
    }
}
