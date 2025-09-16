using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class edittblmnAccountSaleOffice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_T_MN_ACCOUNT_SALE_OFFICE",
                table: "T_MN_ACCOUNT_SALE_OFFICE");

            migrationBuilder.DropIndex(
                name: "IX_T_MN_ACCOUNT_SALE_OFFICE_USER_NAME",
                table: "T_MN_ACCOUNT_SALE_OFFICE");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "T_MN_ACCOUNT_SALE_OFFICE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_MN_ACCOUNT_SALE_OFFICE",
                table: "T_MN_ACCOUNT_SALE_OFFICE",
                columns: new[] { "USER_NAME", "PARTNER_ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_T_MN_ACCOUNT_SALE_OFFICE",
                table: "T_MN_ACCOUNT_SALE_OFFICE");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "T_MN_ACCOUNT_SALE_OFFICE",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0)
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_MN_ACCOUNT_SALE_OFFICE",
                table: "T_MN_ACCOUNT_SALE_OFFICE",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_ACCOUNT_SALE_OFFICE_USER_NAME",
                table: "T_MN_ACCOUNT_SALE_OFFICE",
                column: "USER_NAME");
        }
    }
}
