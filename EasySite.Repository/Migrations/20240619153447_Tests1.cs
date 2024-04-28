using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EasySite.Repository.Migrations
{
    public partial class Tests1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManagerPermitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViewOrders = table.Column<bool>(type: "bit", nullable: false),
                    EditStatusOrder = table.Column<bool>(type: "bit", nullable: false),
                    AddProduct = table.Column<bool>(type: "bit", nullable: false),
                    UpdateProduct = table.Column<bool>(type: "bit", nullable: false),
                    DeleteProduct = table.Column<bool>(type: "bit", nullable: false),
                    AddDepartment = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDepartment = table.Column<bool>(type: "bit", nullable: false),
                    DeleteDepartment = table.Column<bool>(type: "bit", nullable: false),
                    UpdateSite = table.Column<bool>(type: "bit", nullable: false),
                    UpdateHomePage = table.Column<bool>(type: "bit", nullable: false),
                    UpdateSittingFormOrder = table.Column<bool>(type: "bit", nullable: false),
                    DeleteShippingGovernorates = table.Column<bool>(type: "bit", nullable: false),
                    AddDeleteShippingGovernorates = table.Column<bool>(type: "bit", nullable: false),
                    AddRating = table.Column<bool>(type: "bit", nullable: false),
                    UpdateRating = table.Column<bool>(type: "bit", nullable: false),
                    DeleteRating = table.Column<bool>(type: "bit", nullable: false),
                    AppUserId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerPermitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagerPermitions_AppUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerPermitions_UserId",
                table: "ManagerPermitions",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerPermitions");
        }
    }
}
