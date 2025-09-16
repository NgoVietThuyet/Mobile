using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class order_cfg_11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_BUY",
                table: "T_SO_ORDER");

            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_SELL",
                table: "T_SO_ORDER");

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_BUY",
                table: "T_SO_ORDER",
                column: "PARTNER_ID_BUY",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_SELL",
                table: "T_SO_ORDER",
                column: "PARTNER_ID_SELL",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_BUY",
                table: "T_SO_ORDER");

            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_SELL",
                table: "T_SO_ORDER");

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_BUY",
                table: "T_SO_ORDER",
                column: "PARTNER_ID_BUY",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_SELL",
                table: "T_SO_ORDER",
                column: "PARTNER_ID_SELL",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID");
        }
    }
}
