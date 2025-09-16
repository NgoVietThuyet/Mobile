using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class Syncdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropColumn(
                name: "PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropColumn(
                name: "PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_SELL",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_SELL");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_BUY",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_BUY",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_SELL",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_SELL",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_BUY",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_SELL",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_SELL",
                table: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.AddColumn<int>(
                name: "PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_BUY1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_SELL1");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_BUY1",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_SELL1",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
