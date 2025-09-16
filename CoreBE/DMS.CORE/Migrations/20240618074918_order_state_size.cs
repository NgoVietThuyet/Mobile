using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class order_state_size : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "STATE",
                table: "T_SO_ORDER",
                type: "VARCHAR2(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(10)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "STATE",
                table: "T_SO_ORDER",
                type: "VARCHAR2(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(20)",
                oldNullable: true);
        }
    }
}
