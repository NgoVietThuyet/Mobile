using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class menu_right_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_ADDED",
                table: "T_AD_MENU_RIGHT",
                type: "NUMBER(1)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IS_REMOVED",
                table: "T_AD_MENU_RIGHT",
                type: "NUMBER(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IS_ADDED",
                table: "T_AD_MENU_RIGHT");

            migrationBuilder.DropColumn(
                name: "IS_REMOVED",
                table: "T_AD_MENU_RIGHT");
        }
    }
}
