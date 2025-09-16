using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class updateActionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_AD_ACTIONLOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    ACTION_URL = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    REQUEST_DATA = table.Column<string>(type: "CLOB", nullable: false),
                    REQUEST_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    RESPONSE_DATA = table.Column<string>(type: "CLOB", nullable: false),
                    RESPONSE_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    STATUS_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_ACTIONLOG", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_AD_ACTIONLOG");
        }
    }
}
