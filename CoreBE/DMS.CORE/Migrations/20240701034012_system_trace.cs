using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class system_trace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_AD_SYSTEM_TRACE",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "varchar2(50)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar2(255)", nullable: false),
                    TYPE = table.Column<string>(type: "varchar2(50)", nullable: false),
                    ADDRESS = table.Column<string>(type: "varchar2(255)", nullable: false),
                    INTERVAL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    NOTE = table.Column<string>(type: "nvarchar2(500)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_SYSTEM_TRACE", x => x.CODE);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_AD_SYSTEM_TRACE");
        }
    }
}
