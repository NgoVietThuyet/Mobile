using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class update_driver_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CCCD_NUMBER",
                table: "T_MD_DRIVER",
                type: "VARCHAR2(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)");

            migrationBuilder.AddColumn<string>(
                name: "PHONE_NUMBER",
                table: "T_MD_DRIVER",
                type: "VARCHAR2(20)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PHONE_NUMBER",
                table: "T_MD_DRIVER");

            migrationBuilder.AlterColumn<string>(
                name: "CCCD_NUMBER",
                table: "T_MD_DRIVER",
                type: "VARCHAR2(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)",
                oldNullable: true);
        }
    }
}
