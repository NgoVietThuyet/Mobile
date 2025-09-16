using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class updatedb_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_ACCOUNT_ACCOUNTGROUP_T_AD_ACCOUNTGROUP_TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_ACCOUNT_ACCOUNTGROUP_TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.DropColumn(
                name: "PARTNER_ID_CREATE1",
                table: "T_MD_VEHICLE");

            migrationBuilder.DropColumn(
                name: "VEHICLE_TYPE_CODE1",
                table: "T_MD_VEHICLE");

            migrationBuilder.DropColumn(
                name: "TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PARTNER_ID_CREATE1",
                table: "T_MD_VEHICLE",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VEHICLE_TYPE_CODE1",
                table: "T_MD_VEHICLE",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP",
                type: "RAW(16)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_ACCOUNTGROUP_TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP",
                column: "TblAdAccountGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_ACCOUNT_ACCOUNTGROUP_T_AD_ACCOUNTGROUP_TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP",
                column: "TblAdAccountGroupId",
                principalTable: "T_AD_ACCOUNTGROUP",
                principalColumn: "ID");
        }
    }
}
