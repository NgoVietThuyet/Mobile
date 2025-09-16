using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class update_mn_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_MN_AREA_ITEM",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    ITEM_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    AREA_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_AREA_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_AREA_ITEM_T_MD_AREA_AREA_ID",
                        column: x => x.AREA_ID,
                        principalTable: "T_MD_AREA",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_AREA_ITEM_T_MD_ITEM_ITEM_CODE",
                        column: x => x.ITEM_CODE,
                        principalTable: "T_MD_ITEM",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_AREA_ITEM_AREA_ID",
                table: "T_MN_AREA_ITEM",
                column: "AREA_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_AREA_ITEM_ITEM_CODE",
                table: "T_MN_AREA_ITEM",
                column: "ITEM_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MN_AREA_ITEM");
        }
    }
}
