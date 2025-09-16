using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class updatedb_masterdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_DISTRICT_T_MD_PROVINE_PROVINE_ID1",
                table: "T_MD_DISTRICT");

            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_WARD_T_MD_DISTRICT_DISTRICT_ID1",
                table: "T_MD_WARD");

            migrationBuilder.DropIndex(
                name: "IX_T_MD_WARD_DISTRICT_ID1",
                table: "T_MD_WARD");

            migrationBuilder.DropIndex(
                name: "IX_T_MD_DISTRICT_PROVINE_ID1",
                table: "T_MD_DISTRICT");

            migrationBuilder.DropColumn(
                name: "DISTRICT_ID1",
                table: "T_MD_WARD");

            migrationBuilder.DropColumn(
                name: "PROVINE_ID1",
                table: "T_MD_DISTRICT");

            migrationBuilder.AlterColumn<int>(
                name: "DISTRICT_ID",
                table: "T_MD_WARD",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PROVINE_ID",
                table: "T_MD_DISTRICT",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_WARD_DISTRICT_ID",
                table: "T_MD_WARD",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_DISTRICT_PROVINE_ID",
                table: "T_MD_DISTRICT",
                column: "PROVINE_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_DISTRICT_T_MD_PROVINE_PROVINE_ID",
                table: "T_MD_DISTRICT",
                column: "PROVINE_ID",
                principalTable: "T_MD_PROVINE",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_WARD_T_MD_DISTRICT_DISTRICT_ID",
                table: "T_MD_WARD",
                column: "DISTRICT_ID",
                principalTable: "T_MD_DISTRICT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_DISTRICT_T_MD_PROVINE_PROVINE_ID",
                table: "T_MD_DISTRICT");

            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_WARD_T_MD_DISTRICT_DISTRICT_ID",
                table: "T_MD_WARD");

            migrationBuilder.DropIndex(
                name: "IX_T_MD_WARD_DISTRICT_ID",
                table: "T_MD_WARD");

            migrationBuilder.DropIndex(
                name: "IX_T_MD_DISTRICT_PROVINE_ID",
                table: "T_MD_DISTRICT");

            migrationBuilder.AlterColumn<int>(
                name: "DISTRICT_ID",
                table: "T_MD_WARD",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddColumn<int>(
                name: "DISTRICT_ID1",
                table: "T_MD_WARD",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PROVINE_ID",
                table: "T_MD_DISTRICT",
                type: "NUMBER(10)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddColumn<int>(
                name: "PROVINE_ID1",
                table: "T_MD_DISTRICT",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_WARD_DISTRICT_ID1",
                table: "T_MD_WARD",
                column: "DISTRICT_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_DISTRICT_PROVINE_ID1",
                table: "T_MD_DISTRICT",
                column: "PROVINE_ID1");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_DISTRICT_T_MD_PROVINE_PROVINE_ID1",
                table: "T_MD_DISTRICT",
                column: "PROVINE_ID1",
                principalTable: "T_MD_PROVINE",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_WARD_T_MD_DISTRICT_DISTRICT_ID1",
                table: "T_MD_WARD",
                column: "DISTRICT_ID1",
                principalTable: "T_MD_DISTRICT",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
