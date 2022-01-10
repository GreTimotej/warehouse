using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace web.Migrations
{
    public partial class itemdistributor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DistributorID",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Item_DistributorID",
                table: "Item",
                column: "DistributorID");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Distributor_DistributorID",
                table: "Item",
                column: "DistributorID",
                principalTable: "Distributor",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Distributor_DistributorID",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_DistributorID",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "DistributorID",
                table: "Item");
        }
    }
}
