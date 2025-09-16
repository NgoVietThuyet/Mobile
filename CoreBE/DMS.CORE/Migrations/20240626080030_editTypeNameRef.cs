using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class editTypeNameRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "REFRENCE_ID",
                table: "T_MN_ACCOUNT_CARE_STORE",
                type: "VARCHAR2(40)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(16)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "REFRENCE_ID",
                table: "T_MN_ACCOUNT_CARE_STORE",
                type: "VARCHAR2(16)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(40)");
        }
    }
}
