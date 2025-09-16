using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class Tblpk_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_DRIVER_VEHICLE_T_MD_DRIVER_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.DropColumn(
                name: "TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_PARTNER_ID",
                table: "T_MN_DRIVER_VEHICLE",
                column: "PARTNER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_DRIVER_VEHICLE_T_MD_DRIVER_PARTNER_ID",
                table: "T_MN_DRIVER_VEHICLE",
                column: "PARTNER_ID",
                principalTable: "T_MD_DRIVER",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_DRIVER_VEHICLE_T_MD_DRIVER_PARTNER_ID",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_PARTNER_ID",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.AddColumn<int>(
                name: "TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE",
                column: "TblMdDriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_DRIVER_VEHICLE_T_MD_DRIVER_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE",
                column: "TblMdDriverId",
                principalTable: "T_MD_DRIVER",
                principalColumn: "ID");
        }
    }
}
