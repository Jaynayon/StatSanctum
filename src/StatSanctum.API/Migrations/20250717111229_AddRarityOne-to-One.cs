using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatSanctum.Migrations
{
    /// <inheritdoc />
    public partial class AddRarityOnetoOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "MST_Item",
                newName: "RarityID");

            migrationBuilder.CreateIndex(
                name: "IX_MST_Item_RarityID",
                table: "MST_Item",
                column: "RarityID");

            migrationBuilder.AddForeignKey(
                name: "FK_MST_Item_LKP_Rarity_RarityID",
                table: "MST_Item",
                column: "RarityID",
                principalTable: "LKP_Rarity",
                principalColumn: "RarityID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MST_Item_LKP_Rarity_RarityID",
                table: "MST_Item");

            migrationBuilder.DropIndex(
                name: "IX_MST_Item_RarityID",
                table: "MST_Item");

            migrationBuilder.RenameColumn(
                name: "RarityID",
                table: "MST_Item",
                newName: "Type");
        }
    }
}
