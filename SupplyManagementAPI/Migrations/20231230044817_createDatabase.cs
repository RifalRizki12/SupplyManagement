using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupplyManagementAPI.Migrations
{
    public partial class createDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_m_company",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    foto = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_company", x => x.guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_m_persons",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_persons", x => x.guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "nvarchar(25)", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_m_vendor",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    bidang_usaha = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    jenis_perusahaan = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    status_vendor = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_vendor", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_vendor_tb_m_company_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_company",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    otp = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    is_used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    expired_time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    role_guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    created_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    modified_date = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.guid);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_company_guid",
                        column: x => x.guid,
                        principalTable: "tb_m_company",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_roles_role_guid",
                        column: x => x.role_guid,
                        principalTable: "tb_m_roles",
                        principalColumn: "guid",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "tb_m_company",
                columns: new[] { "guid", "address", "created_date", "email", "foto", "modified_date", "name", "phone_number" },
                values: new object[,]
                {
                    { new Guid("6501a19f-1b9b-49ab-adeb-9d5d4631ea11"), "null", new DateTime(2023, 12, 30, 11, 48, 17, 254, DateTimeKind.Local).AddTicks(7378), "admin@mail.com", "null", null, "Admin", "00000" },
                    { new Guid("f5cd1f99-c521-4dc2-8e31-959f66d5c076"), "null", new DateTime(2023, 12, 30, 11, 48, 17, 254, DateTimeKind.Local).AddTicks(7405), "manager@mail.com", "nul", null, "Manager Logistic", "11111" }
                });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "guid", "created_date", "modified_date", "name" },
                values: new object[,]
                {
                    { new Guid("17b81a69-828f-4963-b4d6-5d3130dc114a"), null, null, "vendor" },
                    { new Guid("184f2911-682c-41d1-aafd-f45958d57ddd"), null, null, "manager" },
                    { new Guid("439853e6-0c0e-4651-8bb0-c22042fcb102"), null, null, "admin" }
                });

            migrationBuilder.InsertData(
                table: "tb_m_accounts",
                columns: new[] { "guid", "created_date", "expired_time", "is_used", "modified_date", "otp", "password", "role_guid", "status" },
                values: new object[] { new Guid("6501a19f-1b9b-49ab-adeb-9d5d4631ea11"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 0, "$2a$12$uVJ1rqZNoFlM5yMA8huJQecDgsY/YUXejKW1UWkf6B46tab6rEnrC", new Guid("439853e6-0c0e-4651-8bb0-c22042fcb102"), 1 });

            migrationBuilder.InsertData(
                table: "tb_m_accounts",
                columns: new[] { "guid", "created_date", "expired_time", "is_used", "modified_date", "otp", "password", "role_guid", "status" },
                values: new object[] { new Guid("f5cd1f99-c521-4dc2-8e31-959f66d5c076"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, 0, "$2a$12$Q8JXjmkY83aCdnjlFNI0SeLjGB6J6PcX0iymKnA0MV8Ku3mpAlYOq", new Guid("184f2911-682c-41d1-aafd-f45958d57ddd"), 1 });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_role_guid",
                table: "tb_m_accounts",
                column: "role_guid");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_company_email",
                table: "tb_m_company",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_company_phone_number",
                table: "tb_m_company",
                column: "phone_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_persons_email",
                table: "tb_m_persons",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_persons");

            migrationBuilder.DropTable(
                name: "tb_m_vendor");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_company");
        }
    }
}
