using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class tbl_comment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_CM_COMMENT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    P_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    TYPE = table.Column<string>(type: "varchar2(255)", nullable: false),
                    CONTENT = table.Column<string>(type: "nvarchar2(1000)", nullable: false),
                    ATTACHMENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CM_COMMENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_CM_COMMENT_T_AD_ACCOUNT_CREATE_BY",
                        column: x => x.CREATE_BY,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME");
                    table.ForeignKey(
                        name: "FK_T_CM_COMMENT_T_AD_ACCOUNT_UPDATE_BY",
                        column: x => x.UPDATE_BY,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME");
                    table.ForeignKey(
                        name: "FK_T_CM_COMMENT_T_CM_ATTACHMENT_ATTACHMENT_ID",
                        column: x => x.ATTACHMENT_ID,
                        principalTable: "T_CM_ATTACHMENT",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_CM_COMMENT_T_CM_COMMENT_P_ID",
                        column: x => x.P_ID,
                        principalTable: "T_CM_COMMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_CM_MODULE_COMMENT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    REFERENCE_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    COMMENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MODULE_TYPE = table.Column<string>(type: "varchar2(50)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CM_MODULE_COMMENT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_CM_MODULE_COMMENT_T_CM_COMMENT_COMMENT_ID",
                        column: x => x.COMMENT_ID,
                        principalTable: "T_CM_COMMENT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_CM_COMMENT_ATTACHMENT_ID",
                table: "T_CM_COMMENT",
                column: "ATTACHMENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CM_COMMENT_CREATE_BY",
                table: "T_CM_COMMENT",
                column: "CREATE_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_CM_COMMENT_P_ID",
                table: "T_CM_COMMENT",
                column: "P_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_CM_COMMENT_UPDATE_BY",
                table: "T_CM_COMMENT",
                column: "UPDATE_BY");

            migrationBuilder.CreateIndex(
                name: "IX_T_CM_MODULE_COMMENT_COMMENT_ID",
                table: "T_CM_MODULE_COMMENT",
                column: "COMMENT_ID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_CM_MODULE_COMMENT");

            migrationBuilder.DropTable(
                name: "T_CM_COMMENT");
        }
    }
}
