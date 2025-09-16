using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class update_driver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_DRIVER_T_AD_ACCOUNT_USER_NAME",
                table: "T_MD_DRIVER");

            migrationBuilder.AlterColumn<string>(
                name: "USER_NAME",
                table: "T_MD_DRIVER",
                type: "VARCHAR2(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");

            migrationBuilder.AlterColumn<string>(
                name: "NOTES",
                table: "T_MD_DRIVER",
                type: "NVARCHAR2(510)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(510)");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_DRIVER_T_AD_ACCOUNT_USER_NAME",
                table: "T_MD_DRIVER",
                column: "USER_NAME",
                principalTable: "T_AD_ACCOUNT",
                principalColumn: "USER_NAME");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_DRIVER_T_AD_ACCOUNT_USER_NAME",
                table: "T_MD_DRIVER");

            migrationBuilder.AlterColumn<string>(
                name: "USER_NAME",
                table: "T_MD_DRIVER",
                type: "VARCHAR2(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NOTES",
                table: "T_MD_DRIVER",
                type: "NVARCHAR2(510)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(510)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_DRIVER_T_AD_ACCOUNT_USER_NAME",
                table: "T_MD_DRIVER",
                column: "USER_NAME",
                principalTable: "T_AD_ACCOUNT",
                principalColumn: "USER_NAME",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
