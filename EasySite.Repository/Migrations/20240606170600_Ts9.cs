using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Ts9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductDataInOrder_ProductVariantsInOrderId1",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ProductVariantsInOrderId1",
                table: "OrderItems",
                newName: "ProductDataInOrderId1");

            migrationBuilder.RenameColumn(
                name: "ProductVariantsInOrderId",
                table: "OrderItems",
                newName: "ProductDataInOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductVariantsInOrderId1",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductDataInOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductDataInOrder_ProductDataInOrderId1",
                table: "OrderItems",
                column: "ProductDataInOrderId1",
                principalTable: "ProductDataInOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductDataInOrder_ProductDataInOrderId1",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "ProductDataInOrderId1",
                table: "OrderItems",
                newName: "ProductVariantsInOrderId1");

            migrationBuilder.RenameColumn(
                name: "ProductDataInOrderId",
                table: "OrderItems",
                newName: "ProductVariantsInOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_ProductDataInOrderId1",
                table: "OrderItems",
                newName: "IX_OrderItems_ProductVariantsInOrderId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductDataInOrder_ProductVariantsInOrderId1",
                table: "OrderItems",
                column: "ProductVariantsInOrderId1",
                principalTable: "ProductDataInOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
