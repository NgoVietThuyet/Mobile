using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class driver_partnercreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ERP_ORDER");

            migrationBuilder.AddColumn<int>(
                name: "PARTNER_ID_CREATE",
                table: "T_MD_DRIVER",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_DRIVER_PARTNER_ID_CREATE",
                table: "T_MD_DRIVER",
                column: "PARTNER_ID_CREATE");

            migrationBuilder.AddForeignKey(
                name: "FK_T_MD_DRIVER_T_MD_PARTNER_PARTNER_ID_CREATE",
                table: "T_MD_DRIVER",
                column: "PARTNER_ID_CREATE",
                principalTable: "T_MD_PARTNER",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_MD_DRIVER_T_MD_PARTNER_PARTNER_ID_CREATE",
                table: "T_MD_DRIVER");

            migrationBuilder.DropIndex(
                name: "IX_T_MD_DRIVER_PARTNER_ID_CREATE",
                table: "T_MD_DRIVER");

            migrationBuilder.DropColumn(
                name: "PARTNER_ID_CREATE",
                table: "T_MD_DRIVER");

            migrationBuilder.CreateTable(
                name: "T_ERP_ORDER",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    AREA_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELIVERY_CODE = table.Column<string>(type: "VARCHAR2(20)", nullable: true),
                    DRIVER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    NOTES = table.Column<string>(type: "NVARCHAR2(500)", nullable: true),
                    ORDER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    PARENT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    PARTNER_ID_BUY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PARTNER_ID_SELL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    REFERENCE_ID = table.Column<Guid>(type: "RAW(16)", nullable: true),
                    STATE = table.Column<string>(type: "VARCHAR2(20)", nullable: true),
                    TYPE = table.Column<string>(type: "VARCHAR2(15)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    VEHICLE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ERP_ORDER", x => x.CODE);
                });
        }
    }
}
