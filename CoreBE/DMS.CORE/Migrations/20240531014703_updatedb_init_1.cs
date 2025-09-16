using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class updatedb_init_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GROUP_ID1",
                table: "T_AD_ACCOUNTGROUP_RIGHT");

            migrationBuilder.DropColumn(
                name: "RIGHT_ID1",
                table: "T_AD_ACCOUNTGROUP_RIGHT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GROUP_ID1",
                table: "T_AD_ACCOUNTGROUP_RIGHT",
                type: "RAW(16)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "RIGHT_ID1",
                table: "T_AD_ACCOUNTGROUP_RIGHT",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");
        }
    }
}
