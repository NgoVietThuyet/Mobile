using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class addtblAccountPlanVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_MN_ACCOUNT_PLAN_VISIT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PLAN_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    YEAR = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MONTH = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_ACCOUNT_PLAN_VISIT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_PLAN_VISIT_T_AD_ACCOUNT_USER_NAME",
                        column: x => x.USER_NAME,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MN_ACCOUNT_CARE_STORE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PLAN_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CONTENTS = table.Column<string>(type: "CLOB", nullable: false),
                    REFRENCE_ID = table.Column<string>(type: "VARCHAR2(16)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_ACCOUNT_CARE_STORE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_CARE_STORE_T_MD_PARTNER_PARTNER_ID",
                        column: x => x.PARTNER_ID,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_CARE_STORE_T_MN_ACCOUNT_PLAN_VISIT_PLAN_ID",
                        column: x => x.PLAN_ID,
                        principalTable: "T_MN_ACCOUNT_PLAN_VISIT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MN_ACCOUNT_PLAN_VISIT_STORE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PLAN_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_ACCOUNT_PLAN_VISIT_STORE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_PLAN_VISIT_STORE_T_MD_PARTNER_PARTNER_ID",
                        column: x => x.PARTNER_ID,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_PLAN_VISIT_STORE_T_MN_ACCOUNT_PLAN_VISIT_PLAN_ID",
                        column: x => x.PLAN_ID,
                        principalTable: "T_MN_ACCOUNT_PLAN_VISIT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_CARE_STORE_PARTNER_ID",
                table: "T_MN_ACCOUNT_CARE_STORE",
                column: "PARTNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_CARE_STORE_PLAN_ID",
                table: "T_MN_ACCOUNT_CARE_STORE",
                column: "PLAN_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_PLAN_VISIT_USER_NAME",
                table: "T_MN_ACCOUNT_PLAN_VISIT",
                column: "USER_NAME");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_PLAN_VISIT_STORE_PARTNER_ID",
                table: "T_MN_ACCOUNT_PLAN_VISIT_STORE",
                column: "PARTNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_PLAN_VISIT_STORE_PLAN_ID",
                table: "T_MN_ACCOUNT_PLAN_VISIT_STORE",
                column: "PLAN_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MN_ACCOUNT_CARE_STORE");

            migrationBuilder.DropTable(
                name: "T_MN_ACCOUNT_PLAN_VISIT_STORE");

            migrationBuilder.DropTable(
                name: "T_MN_ACCOUNT_PLAN_VISIT");
        }
    }
}
