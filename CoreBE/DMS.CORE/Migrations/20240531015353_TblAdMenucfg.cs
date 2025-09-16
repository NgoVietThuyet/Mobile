using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class TblAdMenucfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_MENU_T_AD_RIGHT_RIGHT_ID1",
                table: "T_AD_MENU");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_MENU_RIGHT_ID1",
                table: "T_AD_MENU");

            migrationBuilder.DropColumn(
                name: "RIGHT_ID1",
                table: "T_AD_MENU");

            migrationBuilder.AlterColumn<string>(
                name: "RIGHT_ID",
                table: "T_AD_MENU",
                type: "VARCHAR2(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_MENU_RIGHT_ID",
                table: "T_AD_MENU",
                column: "RIGHT_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_MENU_T_AD_RIGHT_RIGHT_ID",
                table: "T_AD_MENU",
                column: "RIGHT_ID",
                principalTable: "T_AD_RIGHT",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_MENU_T_AD_RIGHT_RIGHT_ID",
                table: "T_AD_MENU");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_MENU_RIGHT_ID",
                table: "T_AD_MENU");

            migrationBuilder.AlterColumn<string>(
                name: "RIGHT_ID",
                table: "T_AD_MENU",
                type: "NVARCHAR2(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RIGHT_ID1",
                table: "T_AD_MENU",
                type: "VARCHAR2(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_MENU_RIGHT_ID1",
                table: "T_AD_MENU",
                column: "RIGHT_ID1");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_MENU_T_AD_RIGHT_RIGHT_ID1",
                table: "T_AD_MENU",
                column: "RIGHT_ID1",
                principalTable: "T_AD_RIGHT",
                principalColumn: "Id");
        }
    }
}
