using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class update_mn_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "T_MD_DRIVER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    CCCD_NUMBER = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    NOTES = table.Column<string>(type: "NVARCHAR2(510)", nullable: false),
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REFERENCE_ID = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_DRIVER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MD_DRIVER_T_AD_ACCOUNT_USER_NAME",
                        column: x => x.USER_NAME,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE",
                column: "TblMdDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_DRIVER_USER_NAME",
                table: "T_MD_DRIVER",
                column: "USER_NAME");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MN_DRIVER_VEHICLE_T_MD_DRIVER_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE",
                column: "TblMdDriverId",
                principalTable: "T_MD_DRIVER",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MN_DRIVER_VEHICLE_T_MD_DRIVER_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.DropTable(
                name: "T_MD_DRIVER");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.DropColumn(
                name: "TblMdDriverId",
                table: "T_MN_DRIVER_VEHICLE");
        }
    }
}
