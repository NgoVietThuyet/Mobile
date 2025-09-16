using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class partner_saletype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_PARTNER_T_MD_SALE_TYPE_SALE_TYPE_CODE",
                table: "T_MD_PARTNER");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_PARTNER_T_MD_SALE_TYPE_SALE_TYPE_CODE",
                table: "T_MD_PARTNER",
                column: "SALE_TYPE_CODE",
                principalTable: "T_MD_SALE_TYPE",
                principalColumn: "CODE",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_PARTNER_T_MD_SALE_TYPE_SALE_TYPE_CODE",
                table: "T_MD_PARTNER");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_PARTNER_T_MD_SALE_TYPE_SALE_TYPE_CODE",
                table: "T_MD_PARTNER",
                column: "SALE_TYPE_CODE",
                principalTable: "T_MD_SALE_TYPE",
                principalColumn: "CODE",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
