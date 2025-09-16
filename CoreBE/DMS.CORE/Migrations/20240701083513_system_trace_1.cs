using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class system_trace_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_AD_SYSTEM_TRACE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_AD_SYSTEM_TRACE",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
