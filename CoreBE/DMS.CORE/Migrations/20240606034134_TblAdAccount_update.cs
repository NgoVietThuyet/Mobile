using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class TblAdAccount_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ACCOUNT_TYPE",
                table: "T_AD_ACCOUNT",
                type: "VARCHAR2(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DRIVER_ID",
                table: "T_AD_ACCOUNT",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PARTNER_ID",
                table: "T_AD_ACCOUNT",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_DRIVER_ID",
                table: "T_AD_ACCOUNT",
                column: "DRIVER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_PARTNER_ID",
                table: "T_AD_ACCOUNT",
                column: "PARTNER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_ACCOUNT_T_MD_DRIVER_DRIVER_ID",
                table: "T_AD_ACCOUNT",
                column: "DRIVER_ID",
                principalTable: "T_MD_DRIVER",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_ACCOUNT_T_MD_PARTNER_PARTNER_ID",
                table: "T_AD_ACCOUNT",
                column: "PARTNER_ID",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_ACCOUNT_T_MD_DRIVER_DRIVER_ID",
                table: "T_AD_ACCOUNT");

            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_ACCOUNT_T_MD_PARTNER_PARTNER_ID",
                table: "T_AD_ACCOUNT");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_ACCOUNT_DRIVER_ID",
                table: "T_AD_ACCOUNT");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_ACCOUNT_PARTNER_ID",
                table: "T_AD_ACCOUNT");

            migrationBuilder.DropColumn(
                name: "ACCOUNT_TYPE",
                table: "T_AD_ACCOUNT");

            migrationBuilder.DropColumn(
                name: "DRIVER_ID",
                table: "T_AD_ACCOUNT");

            migrationBuilder.DropColumn(
                name: "PARTNER_ID",
                table: "T_AD_ACCOUNT");
        }
    }
}
