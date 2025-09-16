using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class erp_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IS_SYNC_ERP",
                table: "T_SO_ORDER",
                type: "NUMBER(1)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "T_ERP_ORDER",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PARTNER_ID_BUY = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PARTNER_ID_SELL = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    AREA_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    VEHICLE_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DRIVER_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    ORDER_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELIVERY_CODE = table.Column<string>(type: "VARCHAR2(20)", nullable: true),
                    TYPE = table.Column<string>(type: "VARCHAR2(15)", nullable: true),
                    PARENT_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    STATE = table.Column<string>(type: "VARCHAR2(20)", nullable: true),
                    NOTES = table.Column<string>(type: "NVARCHAR2(500)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    REFERENCE_ID = table.Column<Guid>(type: "RAW(16)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_ERP_ORDER", x => x.CODE);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_ERP_ORDER");

            migrationBuilder.DropColumn(
                name: "IS_SYNC_ERP",
                table: "T_SO_ORDER");
        }
    }
}
