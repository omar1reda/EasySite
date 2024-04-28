using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemVariantOption_OrderItems_OrderItemId",
                table: "ItemVariantOption");

            migrationBuilder.DropIndex(
                name: "IX_ItemVariantOption_OrderItemId",
                table: "ItemVariantOption");

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "ItemVariantOption");

            migrationBuilder.AddColumn<bool>(
                name: "ProductSoldOut",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantsInOrderId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantsInOrderId",
                table: "ItemVariantOption",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductVariantsInOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productDataId = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantsInOrder", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductVariantsInOrderId",
                table: "OrderItems",
                column: "ProductVariantsInOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemVariantOption_ProductVariantsInOrderId",
                table: "ItemVariantOption",
                column: "ProductVariantsInOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVariantOption_ProductVariantsInOrder_ProductVariantsInOrderId",
                table: "ItemVariantOption",
                column: "ProductVariantsInOrderId",
                principalTable: "ProductVariantsInOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductVariantsInOrder_ProductVariantsInOrderId",
                table: "OrderItems",
                column: "ProductVariantsInOrderId",
                principalTable: "ProductVariantsInOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemVariantOption_ProductVariantsInOrder_ProductVariantsInOrderId",
                table: "ItemVariantOption");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductVariantsInOrder_ProductVariantsInOrderId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductVariantsInOrder");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductVariantsInOrderId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_ItemVariantOption_ProductVariantsInOrderId",
                table: "ItemVariantOption");

            migrationBuilder.DropColumn(
                name: "ProductSoldOut",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductVariantsInOrderId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductVariantsInOrderId",
                table: "ItemVariantOption");

            migrationBuilder.AddColumn<int>(
                name: "OrderItemId",
                table: "ItemVariantOption",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ItemVariantOption_OrderItemId",
                table: "ItemVariantOption",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVariantOption_OrderItems_OrderItemId",
                table: "ItemVariantOption",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
