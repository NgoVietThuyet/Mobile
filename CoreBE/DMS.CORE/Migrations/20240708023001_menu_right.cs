using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class menu_right : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_MENU_T_AD_RIGHT_RIGHT_ID",
                table: "T_AD_MENU");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_MENU_RIGHT_ID",
                table: "T_AD_MENU");

            migrationBuilder.DropColumn(
                name: "RIGHT_ID",
                table: "T_AD_MENU");

            migrationBuilder.CreateTable(
                name: "T_AD_MENU_RIGHT",
                columns: table => new
                {
                    MenuId = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    RightId = table.Column<string>(type: "VARCHAR2(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_MENU_RIGHT", x => new { x.MenuId, x.RightId });
                    table.ForeignKey(
                        name: "FK_T_AD_MENU_RIGHT_T_AD_MENU_MenuId",
                        column: x => x.MenuId,
                        principalTable: "T_AD_MENU",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_AD_MENU_RIGHT_T_AD_RIGHT_RightId",
                        column: x => x.RightId,
                        principalTable: "T_AD_RIGHT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_MENU_RIGHT_RightId",
                table: "T_AD_MENU_RIGHT",
                column: "RightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_AD_MENU_RIGHT");

            migrationBuilder.AddColumn<string>(
                name: "RIGHT_ID",
                table: "T_AD_MENU",
                type: "VARCHAR2(50)",
                nullable: true);

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
    }
}
