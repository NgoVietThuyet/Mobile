using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class Refreshtoken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_REFRESH_TOKEN_T_AD_ACCOUNT_USER_NAME1",
                table: "T_AD_REFRESH_TOKEN");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_REFRESH_TOKEN_USER_NAME1",
                table: "T_AD_REFRESH_TOKEN");

            migrationBuilder.DropColumn(
                name: "USER_NAME1",
                table: "T_AD_REFRESH_TOKEN");

            migrationBuilder.AlterColumn<string>(
                name: "USER_NAME",
                table: "T_AD_REFRESH_TOKEN",
                type: "VARCHAR2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_REFRESH_TOKEN_USER_NAME",
                table: "T_AD_REFRESH_TOKEN",
                column: "USER_NAME");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_REFRESH_TOKEN_T_AD_ACCOUNT_USER_NAME",
                table: "T_AD_REFRESH_TOKEN",
                column: "USER_NAME",
                principalTable: "T_AD_ACCOUNT",
                principalColumn: "USER_NAME",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_AD_REFRESH_TOKEN_T_AD_ACCOUNT_USER_NAME",
                table: "T_AD_REFRESH_TOKEN");

            migrationBuilder.DropIndex(
                name: "IX_T_AD_REFRESH_TOKEN_USER_NAME",
                table: "T_AD_REFRESH_TOKEN");

            migrationBuilder.AlterColumn<string>(
                name: "USER_NAME",
                table: "T_AD_REFRESH_TOKEN",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");

            migrationBuilder.AddColumn<string>(
                name: "USER_NAME1",
                table: "T_AD_REFRESH_TOKEN",
                type: "VARCHAR2(50)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_REFRESH_TOKEN_USER_NAME1",
                table: "T_AD_REFRESH_TOKEN",
                column: "USER_NAME1");

            migrationBuilder.AddForeignKey(
                name: "FK_T_AD_REFRESH_TOKEN_T_AD_ACCOUNT_USER_NAME1",
                table: "T_AD_REFRESH_TOKEN",
                column: "USER_NAME1",
                principalTable: "T_AD_ACCOUNT",
                principalColumn: "USER_NAME");
        }
    }
}
