using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Tests4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CrossSelling_CrossSellingId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CrossSellingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CrossSellingId",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CrossSelling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CrossSelling_ProductId",
                table: "CrossSelling",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CrossSelling_Products_ProductId",
                table: "CrossSelling",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CrossSelling_Products_ProductId",
                table: "CrossSelling");

            migrationBuilder.DropIndex(
                name: "IX_CrossSelling_ProductId",
                table: "CrossSelling");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CrossSelling");

            migrationBuilder.AddColumn<int>(
                name: "CrossSellingId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
