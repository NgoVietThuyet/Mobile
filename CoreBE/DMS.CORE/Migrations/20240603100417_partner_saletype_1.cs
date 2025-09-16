using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class partner_saletype_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SALE_TYPE_CODE",
                table: "T_MD_PARTNER",
                type: "VARCHAR2(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SALE_TYPE_CODE",
                table: "T_MD_PARTNER",
                type: "VARCHAR2(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldNullable: true);
        }
    }
}
