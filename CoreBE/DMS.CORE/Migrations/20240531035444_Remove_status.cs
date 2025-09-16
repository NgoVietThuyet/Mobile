using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class Remove_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_WARD");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_VEHICLE_TYPE");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_VEHICLE");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_UNIT");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_TYPE_ITEM");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_PROVINE");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_PARTNER");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_GROUP_ITEM");

            migrationBuilder.DropColumn(
                name: "STATUS",
                table: "T_MD_DISTRICT");

            migrationBuilder.AddColumn<string>(
                name: "NAME",
                table: "T_MD_PARTNER",
                type: "NVARCHAR2(255)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NAME",
                table: "T_MD_PARTNER");

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_WARD",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_VEHICLE_TYPE",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_VEHICLE",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_UNIT",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_TYPE_ITEM",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_PROVINE",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_PARTNER",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_GROUP_ITEM",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "STATUS",
                table: "T_MD_DISTRICT",
                type: "NUMBER(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
