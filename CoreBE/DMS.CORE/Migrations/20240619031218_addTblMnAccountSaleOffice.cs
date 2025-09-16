using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class addTblMnAccountSaleOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "USER_TYPE",
                table: "T_AD_ACCOUNT",
                type: "VARCHAR2(255)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "T_MN_ACCOUNT_SALE_OFFICE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_ACCOUNT_SALE_OFFICE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_SALE_OFFICE_T_AD_ACCOUNT_USER_NAME",
                        column: x => x.USER_NAME,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_ACCOUNT_SALE_OFFICE_T_MD_PARTNER_PARTNER_ID",
                        column: x => x.PARTNER_ID,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_SALE_OFFICE_PARTNER_ID",
                table: "T_MN_ACCOUNT_SALE_OFFICE",
                column: "PARTNER_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_SALE_OFFICE_USER_NAME",
                table: "T_MN_ACCOUNT_SALE_OFFICE",
                column: "USER_NAME");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MN_ACCOUNT_SALE_OFFICE");

            migrationBuilder.DropColumn(
                name: "USER_TYPE",
                table: "T_AD_ACCOUNT");
        }
    }
}
