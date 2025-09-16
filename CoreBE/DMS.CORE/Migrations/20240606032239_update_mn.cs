using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class update_mn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MD_DRIVER");

            migrationBuilder.CreateTable(
                name: "T_MN_DRIVER_VEHICLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    VEHICLE_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_DRIVER_VEHICLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_DRIVER_VEHICLE_T_MD_VEHICLE_VEHICLE_CODE",
                        column: x => x.VEHICLE_CODE,
                        principalTable: "T_MD_VEHICLE",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MN_PARTNER_AREA",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    AREA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_PARTNER_AREA", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_AREA_T_MD_AREA_AREA_ID",
                        column: x => x.AREA_ID,
                        principalTable: "T_MD_AREA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_AREA_T_MD_PARTNER_PARTNER_ID",
                        column: x => x.PARTNER_ID,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_DRIVER_VEHICLE_VEHICLE_CODE",
                table: "T_MN_DRIVER_VEHICLE",
                column: "VEHICLE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_AREA_AREA_ID",
                table: "T_MN_PARTNER_AREA",
                column: "AREA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_AREA_PARTNER_ID",
                table: "T_MN_PARTNER_AREA",
                column: "PARTNER_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MN_DRIVER_VEHICLE");

            migrationBuilder.DropTable(
                name: "T_MN_PARTNER_AREA");

            migrationBuilder.CreateTable(
                name: "T_MD_DRIVER",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    CCCD_NUMBER = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTES = table.Column<string>(type: "NVARCHAR2(510)", nullable: false),
                    REFERENCE_ID = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
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
                name: "IX_T_MD_DRIVER_USER_NAME",
                table: "T_MD_DRIVER",
                column: "USER_NAME");
        }
    }
}
