using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatSanctum.Migrations
{
    /// <inheritdoc />
    public partial class addLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Equipments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Equipments");
        }
    }
}
