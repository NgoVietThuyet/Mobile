using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class tblmd_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_MD_ITEM",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    GROUP_ITEM_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    TYPE_ITEM_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_ITEM", x => x.CODE);
                    table.ForeignKey(
                        name: "FK_T_MD_ITEM_T_MD_GROUP_ITEM_GROUP_ITEM_CODE",
                        column: x => x.GROUP_ITEM_CODE,
                        principalTable: "T_MD_GROUP_ITEM",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_MD_ITEM_T_MD_TYPE_ITEM_TYPE_ITEM_CODE",
                        column: x => x.TYPE_ITEM_CODE,
                        principalTable: "T_MD_TYPE_ITEM",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_ITEM_GROUP_ITEM_CODE",
                table: "T_MD_ITEM",
                column: "GROUP_ITEM_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_ITEM_TYPE_ITEM_CODE",
                table: "T_MD_ITEM",
                column: "TYPE_ITEM_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_MD_ITEM");
        }
    }
}
