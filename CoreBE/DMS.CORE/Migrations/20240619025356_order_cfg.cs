using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class order_cfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_SO_ORDER_PARENT_CODE",
                table: "T_SO_ORDER");

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_SO_ORDER_PARENT_CODE",
                table: "T_SO_ORDER",
                column: "PARENT_CODE",
                principalTable: "T_SO_ORDER",
                principalColumn: "CODE",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_SO_ORDER_PARENT_CODE",
                table: "T_SO_ORDER");

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_SO_ORDER_PARENT_CODE",
                table: "T_SO_ORDER",
                column: "PARENT_CODE",
                principalTable: "T_SO_ORDER",
                principalColumn: "CODE");
        }
    }
}
