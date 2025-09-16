using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class tbl_contract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CONTRACT_ID",
                table: "T_SO_ORDER",
                type: "VARCHAR2(50)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "T_MN_CONTRACT",
                columns: table => new
                {
                    CONTRACT_ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_CONTRACT", x => x.CONTRACT_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MN_PARTNER_CONTRACT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CONTRACT_ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_PARTNER_CONTRACT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_CONTRACT_T_MD_PARTNER_PARTNER_ID",
                        column: x => x.PARTNER_ID,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_CONTRACT_T_MN_CONTRACT_CONTRACT_ID",
                        column: x => x.CONTRACT_ID,
                        principalTable: "T_MN_CONTRACT",
                        principalColumn: "CONTRACT_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_SO_ORDER_CONTRACT_ID",
                table: "T_SO_ORDER",
                column: "CONTRACT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_CONTRACT_CONTRACT_ID",
                table: "T_MN_PARTNER_CONTRACT",
                column: "CONTRACT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_CONTRACT_PARTNER_ID",
                table: "T_MN_PARTNER_CONTRACT",
                column: "PARTNER_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_T_SO_ORDER_T_MN_CONTRACT_CONTRACT_ID",
                table: "T_SO_ORDER",
                column: "CONTRACT_ID",
                principalTable: "T_MN_CONTRACT",
                principalColumn: "CONTRACT_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_T_SO_ORDER_T_MN_CONTRACT_CONTRACT_ID",
                table: "T_SO_ORDER");

            migrationBuilder.DropTable(
                name: "T_MN_PARTNER_CONTRACT");

            migrationBuilder.DropTable(
                name: "T_MN_CONTRACT");

            migrationBuilder.DropIndex(
                name: "IX_T_SO_ORDER_CONTRACT_ID",
                table: "T_SO_ORDER");

            migrationBuilder.DropColumn(
                name: "CONTRACT_ID",
                table: "T_SO_ORDER");
        }
    }
}
