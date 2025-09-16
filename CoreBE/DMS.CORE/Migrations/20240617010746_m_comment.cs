using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class m_comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MODULE_TYPE",
                table: "T_CM_MODULE_COMMENT",
                type: "varchar2(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar2(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MODULE_TYPE",
                table: "T_CM_MODULE_COMMENT",
                type: "varchar2(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar2(50)",
                oldNullable: true);
        }
    }
}
