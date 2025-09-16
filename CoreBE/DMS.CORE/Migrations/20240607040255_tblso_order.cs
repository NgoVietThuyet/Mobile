using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class tblso_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_SO_ORDER",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PARTNER_ID_BUY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PARTNER_ID_SELL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    AREA_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VEHICLE_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    DRIVER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORDER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELIVERY_CODE = table.Column<string>(type: "VARCHAR2(20)", nullable: true),
                    TYPE = table.Column<string>(type: "VARCHAR2(15)", nullable: true),
                    PARENT_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    State = table.Column<string>(type: "VARCHAR2(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REFERENCE_ID = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SO_ORDER", x => x.CODE);
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_T_MD_AREA_AREA_ID",
                        column: x => x.AREA_ID,
                        principalTable: "T_MD_AREA",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_T_MD_DRIVER_DRIVER_ID",
                        column: x => x.DRIVER_ID,
                        principalTable: "T_MD_DRIVER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_BUY",
                        column: x => x.PARTNER_ID_BUY,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_T_MD_PARTNER_PARTNER_ID_SELL",
                        column: x => x.PARTNER_ID_SELL,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_T_MD_VEHICLE_VEHICLE_CODE",
                        column: x => x.VEHICLE_CODE,
                        principalTable: "T_MD_VEHICLE",
                        principalColumn: "CODE");
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_T_SO_ORDER_PARENT_CODE",
                        column: x => x.PARENT_CODE,
                        principalTable: "T_SO_ORDER",
                        principalColumn: "CODE");
                });

            migrationBuilder.CreateTable(
                name: "T_SO_ORDER_DETAIL",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ORDER_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    ITEM_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    ORDER_NUMBER = table.Column<double>(type: "BINARY_DOUBLE", nullable: false),
                    UNIT_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SO_ORDER_DETAIL", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_DETAIL_T_MD_ITEM_ITEM_CODE",
                        column: x => x.ITEM_CODE,
                        principalTable: "T_MD_ITEM",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_DETAIL_T_MD_UNIT_UNIT_CODE",
                        column: x => x.UNIT_CODE,
                        principalTable: "T_MD_UNIT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_DETAIL_T_SO_ORDER_ORDER_CODE",
                        column: x => x.ORDER_CODE,
                        principalTable: "T_SO_ORDER",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_SO_ORDER_PROCESS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ORDER_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PROCESS_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    ACTION_CODE = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    PREV_STATE = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    STATE = table.Column<string>(type: "VARCHAR2(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SO_ORDER_PROCESS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_SO_ORDER_PROCESS_T_SO_ORDER_ORDER_CODE",
                        column: x => x.ORDER_CODE,
                        principalTable: "T_SO_ORDER",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_AREA_ID",
                table: "T_SO_ORDER",
                column: "AREA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_DRIVER_ID",
                table: "T_SO_ORDER",
                column: "DRIVER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_PARENT_CODE",
                table: "T_SO_ORDER",
                column: "PARENT_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_PARTNER_ID_BUY",
                table: "T_SO_ORDER",
                column: "PARTNER_ID_BUY");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_PARTNER_ID_SELL",
                table: "T_SO_ORDER",
                column: "PARTNER_ID_SELL");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_VEHICLE_CODE",
                table: "T_SO_ORDER",
                column: "VEHICLE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_DETAIL_ITEM_CODE",
                table: "T_SO_ORDER_DETAIL",
                column: "ITEM_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_DETAIL_ORDER_CODE",
                table: "T_SO_ORDER_DETAIL",
                column: "ORDER_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_DETAIL_UNIT_CODE",
                table: "T_SO_ORDER_DETAIL",
                column: "UNIT_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_PROCESS_ORDER_CODE",
                table: "T_SO_ORDER_PROCESS",
                column: "ORDER_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_SO_ORDER_DETAIL");

            migrationBuilder.DropTable(
                name: "T_SO_ORDER_PROCESS");

            migrationBuilder.DropTable(
                name: "T_SO_ORDER");
        }
    }
}
