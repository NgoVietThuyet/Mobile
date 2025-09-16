using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class addColumnActionDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ACTION_DATE",
                table: "T_MN_ACCOUNT_CARE_STORE",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACTION_DATE",
                table: "T_MN_ACCOUNT_CARE_STORE");
        }
    }
}
