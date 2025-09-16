using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class updatetblActionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RESPONSE_TIME",
                table: "T_AD_ACTIONLOG",
                type: "TIMESTAMP(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "REQUEST_TIME",
                table: "T_AD_ACTIONLOG",
                type: "TIMESTAMP(7)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "RESPONSE_TIME",
                table: "T_AD_ACTIONLOG",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "REQUEST_TIME",
                table: "T_AD_ACTIONLOG",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMP(7)",
                oldNullable: true);
        }
    }
}
