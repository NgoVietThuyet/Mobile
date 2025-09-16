using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class update_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "State",
            //    table: "T_SO_ORDER",
            //    newName: "STATE");

            migrationBuilder.AddColumn<string>(
                name: "NOTES",
                table: "T_SO_ORDER",
                type: "NVARCHAR2(500)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NOTES",
                table: "T_SO_ORDER");

            //migrationBuilder.RenameColumn(
            //    name: "STATE",
            //    table: "T_SO_ORDER",
            //    newName: "State");
        }
    }
}
