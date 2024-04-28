using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "ProductVariantsInOrderId1",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ProductVariantsInOrderId",
                table: "ItemVariantOption",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductDataInOrderId",
                table: "ItemVariantOption",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductDataInOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productDataId = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDataInOrder", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductVariantsInOrderId1",
                table: "OrderItems",
                column: "ProductVariantsInOrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_ItemVariantOption_ProductDataInOrderId",
                table: "ItemVariantOption",
                column: "ProductDataInOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemVariantOption_ProductDataInOrder_ProductDataInOrderId",
                table: "ItemVariantOption",
                column: "ProductDataInOrderId",
                principalTable: "ProductDataInOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductDataInOrder_ProductVariantsInOrderId1",
                table: "OrderItems",
                column: "ProductVariantsInOrderId1",
                principalTable: "ProductDataInOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemVariantOption_ProductDataInOrder_ProductDataInOrderId",
                table: "ItemVariantOption");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductDataInOrder_ProductVariantsInOrderId1",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "ProductDataInOrder");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductVariantsInOrderId1",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_ItemVariantOption_ProductDataInOrderId",
                table: "ItemVariantOption");

            migrationBuilder.DropColumn(
                name: "ProductVariantsInOrderId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductDataInOrderId",
                table: "ItemVariantOption");

            migrationBuilder.AlterColumn<int>(
                name: "ProductVariantsInOrderId",
                table: "ItemVariantOption",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ProductVariantsInOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderItemId = table.Column<int>(type: "int", nullable: false),
                    productDataId = table.Column<int>(type: "int", nullable: false)
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
    }
}
