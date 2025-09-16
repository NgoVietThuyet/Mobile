using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class Updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GROUP_ID1",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.DropColumn(
                name: "USER_NAME1",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_ACCOUNT_ACCOUNTGROUP_T_AD_ACCOUNTGROUP_TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_ACCOUNT_ACCOUNTGROUP_TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.DropColumn(
                name: "TblAdAccountGroupId",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.AddColumn<Guid>(
                name: "GROUP_ID1",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP",
                type: "RAW(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "USER_NAME1",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }
    }
}
