using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class system_trace_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LOG",
                table: "T_AD_SYSTEM_TRACE",
                type: "nvarchar2(500)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_AD_SYSTEM_TRACE",
                type: "NUMBER(1)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LOG",
                table: "T_AD_SYSTEM_TRACE");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_AD_SYSTEM_TRACE");
        }
    }
}
