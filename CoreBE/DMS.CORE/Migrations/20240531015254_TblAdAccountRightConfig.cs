using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class TblAdAccountRightConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_ACCOUNT_RIGHT_T_AD_ACCOUNT_USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_ACCOUNT_RIGHT_USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT");

            migrationBuilder.DropColumn(
                name: "RIGHT_ID1",
                table: "T_AD_ACCOUNT_RIGHT");

            migrationBuilder.DropColumn(
                name: "USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT");

            migrationBuilder.AlterColumn<string>(
                name: "USER_NAME",
                table: "T_AD_ACCOUNT_RIGHT",
                type: "VARCHAR2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_ACCOUNT_RIGHT_T_AD_ACCOUNT_USER_NAME",
                table: "T_AD_ACCOUNT_RIGHT",
                column: "USER_NAME",
                principalTable: "T_AD_ACCOUNT",
                principalColumn: "USER_NAME",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_ACCOUNT_RIGHT_T_AD_ACCOUNT_USER_NAME",
                table: "T_AD_ACCOUNT_RIGHT");

            migrationBuilder.AlterColumn<string>(
                name: "USER_NAME",
                table: "T_AD_ACCOUNT_RIGHT",
                type: "NVARCHAR2(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");

            migrationBuilder.AddColumn<string>(
                name: "RIGHT_ID1",
                table: "T_AD_ACCOUNT_RIGHT",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT",
                type: "VARCHAR2(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_RIGHT_USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT",
                column: "USER_NAME1");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_ACCOUNT_RIGHT_T_AD_ACCOUNT_USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT",
                column: "USER_NAME1",
                principalTable: "T_AD_ACCOUNT",
                principalColumn: "USER_NAME");
        }
    }
}
