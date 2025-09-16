using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DMS.CORE.Migrations
{
    /// <inheritdoc />
    public partial class init_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "T_AD_ACCOUNT",
                columns: table => new
                {
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    FULL_NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    PASSWORD = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PHONE_NUMBER = table.Column<string>(type: "VARCHAR2(10)", nullable: true),
                    EMAIL = table.Column<string>(type: "VARCHAR2(255)", nullable: true),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(255)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELETE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_ACCOUNT", x => x.USER_NAME);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_ACCOUNTGROUP",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    NAME = table.Column<string>(type: "nVARCHAR2(50)", nullable: false),
                    NOTES = table.Column<string>(type: "nVARCHAR2(255)", nullable: true),
                    ROLE_CODE = table.Column<string>(type: "VARCHAR2(255)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELETE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_ACCOUNTGROUP", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_APP_VERSION",
                columns: table => new
                {
                    VERSION_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VERSION_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_REQUIRED_UPDATE = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_APP_VERSION", x => x.VERSION_CODE);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_MESSAGE",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(10)", nullable: false),
                    LANG = table.Column<string>(type: "VARCHAR2(10)", nullable: false),
                    VALUE = table.Column<string>(type: "nVARCHAR2(255)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELETE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_MESSAGE", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_RIGHT",
                columns: table => new
                {
                    Id = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    Name = table.Column<string>(type: "nVARCHAR2(50)", nullable: false),
                    PId = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELETE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_RIGHT", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_GROUP_ITEM",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_GROUP_ITEM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_PROVINE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_PROVINE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_SALE_TYPE",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(155)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_SALE_TYPE", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_TYPE_ITEM",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_TYPE_ITEM", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_UNIT",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_UNIT", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_VEHICLE_TYPE",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "VARCHAR2(155)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_VEHICLE_TYPE", x => x.CODE);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_REFRESH_TOKEN",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    USER_NAME = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    REFRESH_TOKEN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    EXPIRE_TIME = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    USER_NAME1 = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_REFRESH_TOKEN", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_AD_REFRESH_TOKEN_T_AD_ACCOUNT_USER_NAME1",
                        column: x => x.USER_NAME1,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME");
                });

            migrationBuilder.CreateTable(
                name: "T_AD_ACCOUNT_ACCOUNTGROUP",
                columns: table => new
                {
                    USER_NAME = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    GROUP_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    GROUP_ID1 = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    USER_NAME1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_ACCOUNT_ACCOUNTGROUP", x => new { x.USER_NAME, x.GROUP_ID });
                    table.ForeignKey(
                        name: "FK_T_AD_ACCOUNT_ACCOUNTGROUP_T_AD_ACCOUNTGROUP_GROUP_ID",
                        column: x => x.GROUP_ID,
                        principalTable: "T_AD_ACCOUNTGROUP",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_AD_ACCOUNT_ACCOUNTGROUP_T_AD_ACCOUNT_USER_NAME",
                        column: x => x.USER_NAME,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_ACCOUNTGROUP_RIGHT",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    GROUP_ID = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    RIGHT_ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    GROUP_ID1 = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    RIGHT_ID1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELETE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_ACCOUNTGROUP_RIGHT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_AD_ACCOUNTGROUP_RIGHT_T_AD_ACCOUNTGROUP_GROUP_ID",
                        column: x => x.GROUP_ID,
                        principalTable: "T_AD_ACCOUNTGROUP",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_AD_ACCOUNTGROUP_RIGHT_T_AD_RIGHT_RIGHT_ID",
                        column: x => x.RIGHT_ID,
                        principalTable: "T_AD_RIGHT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_ACCOUNT_RIGHT",
                columns: table => new
                {
                    USER_NAME = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    RIGHT_ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_ADDED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    IS_REMOVED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    USER_NAME1 = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    RIGHT_ID1 = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_ACCOUNT_RIGHT", x => new { x.USER_NAME, x.RIGHT_ID });
                    table.ForeignKey(
                        name: "FK_T_AD_ACCOUNT_RIGHT_T_AD_ACCOUNT_USER_NAME1",
                        column: x => x.USER_NAME1,
                        principalTable: "T_AD_ACCOUNT",
                        principalColumn: "USER_NAME");
                    table.ForeignKey(
                        name: "FK_T_AD_ACCOUNT_RIGHT_T_AD_RIGHT_RIGHT_ID",
                        column: x => x.RIGHT_ID,
                        principalTable: "T_AD_RIGHT",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_AD_MENU",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "nVARCHAR2(255)", nullable: false),
                    P_ID = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    RIGHT_ID = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    URL = table.Column<string>(type: "VARCHAR2(255)", nullable: true),
                    ICON = table.Column<string>(type: "VARCHAR2(255)", nullable: true),
                    RIGHT_ID1 = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    DELETE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AD_MENU", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_AD_MENU_T_AD_RIGHT_RIGHT_ID1",
                        column: x => x.RIGHT_ID1,
                        principalTable: "T_AD_RIGHT",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "T_MD_DISTRICT",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PROVINE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PROVINE_ID1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_DISTRICT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MD_DISTRICT_T_MD_PROVINE_PROVINE_ID1",
                        column: x => x.PROVINE_ID1,
                        principalTable: "T_MD_PROVINE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_WARD",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    NAME = table.Column<string>(type: "NVARCHAR2(255)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ORDER_NUMBER = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DISTRICT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DISTRICT_ID1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_WARD", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MD_WARD_T_MD_DISTRICT_DISTRICT_ID1",
                        column: x => x.DISTRICT_ID1,
                        principalTable: "T_MD_DISTRICT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MD_PARTNER",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    SALE_TYPE_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    IS_CUSTOMER = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    IS_SUPPLIER = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    ADDRESS = table.Column<string>(type: "NVARCHAR2(255)", nullable: true),
                    PHONE_NUMBER = table.Column<string>(type: "VARCHAR2(15)", nullable: true),
                    EMAIL = table.Column<string>(type: "VARCHAR2(155)", nullable: true),
                    PROVINCE_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    DISTRICT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    WARD_ID = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    DESCRIPTION = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LONGITUDE = table.Column<string>(type: "VARCHAR2(15)", nullable: true),
                    LATITUDE = table.Column<string>(type: "VARCHAR2(15)", nullable: true),
                    REPRESENTATIVE = table.Column<string>(type: "VARCHAR2(155)", nullable: true),
                    TAX_NUMBER = table.Column<string>(type: "VARCHAR2(20)", nullable: true),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_PARTNER", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MD_PARTNER_T_MD_DISTRICT_DISTRICT_ID",
                        column: x => x.DISTRICT_ID,
                        principalTable: "T_MD_DISTRICT",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_MD_PARTNER_T_MD_PROVINE_PROVINCE_ID",
                        column: x => x.PROVINCE_ID,
                        principalTable: "T_MD_PROVINE",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_MD_PARTNER_T_MD_SALE_TYPE_SALE_TYPE_CODE",
                        column: x => x.SALE_TYPE_CODE,
                        principalTable: "T_MD_SALE_TYPE",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MD_PARTNER_T_MD_WARD_WARD_ID",
                        column: x => x.WARD_ID,
                        principalTable: "T_MD_WARD",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "T_MD_VEHICLE",
                columns: table => new
                {
                    CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    VEHICLE_TYPE_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    STATUS = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    TONNAGE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    TARE_TONNAGE = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    HEIGHT = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    WIDTH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    LENGTH = table.Column<double>(type: "BINARY_DOUBLE", nullable: true),
                    PARTNER_ID_CREATE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PARTNER_ID_CREATE1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VEHICLE_TYPE_CODE1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MD_VEHICLE", x => x.CODE);
                    table.ForeignKey(
                        name: "FK_T_MD_VEHICLE_T_MD_PARTNER_PARTNER_ID_CREATE",
                        column: x => x.PARTNER_ID_CREATE,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_T_MD_VEHICLE_T_MD_VEHICLE_TYPE_VEHICLE_TYPE_CODE",
                        column: x => x.VEHICLE_TYPE_CODE,
                        principalTable: "T_MD_VEHICLE_TYPE",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MN_PARTNER_REFERENCE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    PARTNER_ID_BUY = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARTNER_ID_SELL = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARTNER_ID_BUY1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    PARTNER_ID_SELL1 = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_PARTNER_REFERENCE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_BUY1",
                        column: x => x.PARTNER_ID_BUY1,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_REFERENCE_T_MD_PARTNER_PARTNER_ID_SELL1",
                        column: x => x.PARTNER_ID_SELL1,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "T_MN_PARTNER_VEHICLE",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    VEHICLE_CODE = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    PARTNER_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IS_ACTIVE = table.Column<bool>(type: "NUMBER(1)", nullable: true),
                    CREATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    UPDATE_BY = table.Column<string>(type: "VARCHAR2(50)", nullable: true),
                    CREATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_MN_PARTNER_VEHICLE", x => x.ID);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_VEHICLE_T_MD_PARTNER_PARTNER_ID",
                        column: x => x.PARTNER_ID,
                        principalTable: "T_MD_PARTNER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_T_MN_PARTNER_VEHICLE_T_MD_VEHICLE_VEHICLE_CODE",
                        column: x => x.VEHICLE_CODE,
                        principalTable: "T_MD_VEHICLE",
                        principalColumn: "CODE",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNTGROUP_RIGHT_GROUP_ID",
                table: "T_AD_ACCOUNTGROUP_RIGHT",
                column: "GROUP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNTGROUP_RIGHT_RIGHT_ID",
                table: "T_AD_ACCOUNTGROUP_RIGHT",
                column: "RIGHT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_ACCOUNTGROUP_GROUP_ID",
                table: "T_AD_ACCOUNT_ACCOUNTGROUP",
                column: "GROUP_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_RIGHT_RIGHT_ID",
                table: "T_AD_ACCOUNT_RIGHT",
                column: "RIGHT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_ACCOUNT_RIGHT_USER_NAME1",
                table: "T_AD_ACCOUNT_RIGHT",
                column: "USER_NAME1");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_MENU_RIGHT_ID1",
                table: "T_AD_MENU",
                column: "RIGHT_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_T_AD_REFRESH_TOKEN_USER_NAME1",
                table: "T_AD_REFRESH_TOKEN",
                column: "USER_NAME1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_DISTRICT_PROVINE_ID1",
                table: "T_MD_DISTRICT",
                column: "PROVINE_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_PARTNER_CODE",
                table: "T_MD_PARTNER",
                column: "CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_PARTNER_DISTRICT_ID",
                table: "T_MD_PARTNER",
                column: "DISTRICT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_PARTNER_PROVINCE_ID",
                table: "T_MD_PARTNER",
                column: "PROVINCE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_PARTNER_SALE_TYPE_CODE",
                table: "T_MD_PARTNER",
                column: "SALE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_PARTNER_WARD_ID",
                table: "T_MD_PARTNER",
                column: "WARD_ID");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_VEHICLE_PARTNER_ID_CREATE",
                table: "T_MD_VEHICLE",
                column: "PARTNER_ID_CREATE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_VEHICLE_VEHICLE_TYPE_CODE",
                table: "T_MD_VEHICLE",
                column: "VEHICLE_TYPE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_T_MD_WARD_DISTRICT_ID1",
                table: "T_MD_WARD",
                column: "DISTRICT_ID1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_BUY1",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_BUY1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_BUY_PARTNER_ID_SELL",
                table: "T_MN_PARTNER_REFERENCE",
                columns: new[] { "PARTNER_ID_BUY", "PARTNER_ID_SELL" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_REFERENCE_PARTNER_ID_SELL1",
                table: "T_MN_PARTNER_REFERENCE",
                column: "PARTNER_ID_SELL1");

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_VEHICLE_PARTNER_ID_VEHICLE_CODE",
                table: "T_MN_PARTNER_VEHICLE",
                columns: new[] { "PARTNER_ID", "VEHICLE_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_MN_PARTNER_VEHICLE_VEHICLE_CODE",
                table: "T_MN_PARTNER_VEHICLE",
                column: "VEHICLE_CODE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_AD_ACCOUNTGROUP_RIGHT");

            migrationBuilder.DropTable(
                name: "T_AD_ACCOUNT_ACCOUNTGROUP");

            migrationBuilder.DropTable(
                name: "T_AD_ACCOUNT_RIGHT");

            migrationBuilder.DropTable(
                name: "T_AD_APP_VERSION");

            migrationBuilder.DropTable(
                name: "T_AD_MENU");

            migrationBuilder.DropTable(
                name: "T_AD_MESSAGE");

            migrationBuilder.DropTable(
                name: "T_AD_REFRESH_TOKEN");

            migrationBuilder.DropTable(
                name: "T_MD_GROUP_ITEM");

            migrationBuilder.DropTable(
                name: "T_MD_TYPE_ITEM");

            migrationBuilder.DropTable(
                name: "T_MD_UNIT");

            migrationBuilder.DropTable(
                name: "T_MN_PARTNER_REFERENCE");

            migrationBuilder.DropTable(
                name: "T_MN_PARTNER_VEHICLE");

            migrationBuilder.DropTable(
                name: "T_AD_ACCOUNTGROUP");

            migrationBuilder.DropTable(
                name: "T_AD_RIGHT");

            migrationBuilder.DropTable(
                name: "T_AD_ACCOUNT");

            migrationBuilder.DropTable(
                name: "T_MD_VEHICLE");

            migrationBuilder.DropTable(
                name: "T_MD_PARTNER");

            migrationBuilder.DropTable(
                name: "T_MD_VEHICLE_TYPE");

            migrationBuilder.DropTable(
                name: "T_MD_SALE_TYPE");

            migrationBuilder.DropTable(
                name: "T_MD_WARD");

            migrationBuilder.DropTable(
                name: "T_MD_DISTRICT");

            migrationBuilder.DropTable(
                name: "T_MD_PROVINE");
        }
    }
}
