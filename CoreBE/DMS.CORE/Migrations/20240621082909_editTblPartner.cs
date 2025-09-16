using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class editTblPartner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LONGITUDE",
                table: "T_MD_PARTNER",
                type: "VARCHAR2(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(15)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LATITUDE",
                table: "T_MD_PARTNER",
                type: "VARCHAR2(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(15)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LONGITUDE",
                table: "T_MD_PARTNER",
                type: "VARCHAR2(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LATITUDE",
                table: "T_MD_PARTNER",
                type: "VARCHAR2(15)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR2(50)",
                oldNullable: true);
        }
    }
}
