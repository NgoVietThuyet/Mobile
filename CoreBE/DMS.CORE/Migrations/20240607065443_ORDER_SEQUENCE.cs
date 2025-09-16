using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class ORDER_SEQUENCE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "ORDER_SEQUENCE");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "T_SO_ORDER",
                type: "VARCHAR2(50)",
                nullable: false,
                defaultValue: "ORDER_SEQUENCE.NEXTVAL",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "ORDER_SEQUENCE");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "T_SO_ORDER",
                type: "VARCHAR2(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldDefaultValue: "ORDER_SEQUENCE.NEXTVAL");
        }
    }
}
