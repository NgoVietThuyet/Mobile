using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class order_process_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "STATE",
                table: "T_SO_ORDER_PROCESS",
                type: "VARCHAR2(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)");

            migrationBuilder.AlterColumn<string>(
                name: "PREV_STATE",
                table: "T_SO_ORDER_PROCESS",
                type: "VARCHAR2(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)");

            migrationBuilder.AlterColumn<string>(
                name: "ACTION_CODE",
                table: "T_SO_ORDER_PROCESS",
                type: "VARCHAR2(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "T_SO_ORDER",
                type: "VARCHAR2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldDefaultValue: "ORDER_SEQUENCE.NEXTVAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "STATE",
                table: "T_SO_ORDER_PROCESS",
                type: "VARCHAR2(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PREV_STATE",
                table: "T_SO_ORDER_PROCESS",
                type: "VARCHAR2(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ACTION_CODE",
                table: "T_SO_ORDER_PROCESS",
                type: "VARCHAR2(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "T_SO_ORDER",
                type: "VARCHAR2(50)",
                nullable: false,
                defaultValue: "ORDER_SEQUENCE.NEXTVAL",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");
        }
    }
}
