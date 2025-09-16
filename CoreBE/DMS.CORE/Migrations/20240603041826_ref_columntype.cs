using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class ref_columntype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MODULE_TYPE",
                table: "T_CM_MODULE_ATTACHMENT",
                type: "VARCHAR2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "T_CM_ATTACHMENT",
                type: "varchar2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar2(255)");

            migrationBuilder.AlterColumn<string>(
                name: "EXTENSION",
                table: "T_CM_ATTACHMENT",
                type: "varchar2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar2(255)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MODULE_TYPE",
                table: "T_CM_MODULE_ATTACHMENT",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");

            migrationBuilder.AlterColumn<string>(
                name: "TYPE",
                table: "T_CM_ATTACHMENT",
                type: "varchar2(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar2(50)");

            migrationBuilder.AlterColumn<string>(
                name: "EXTENSION",
                table: "T_CM_ATTACHMENT",
                type: "varchar2(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar2(50)");
        }
    }
}
