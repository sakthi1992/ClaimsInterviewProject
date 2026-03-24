using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClaimsApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MemberName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimNotes_Claims_ClaimId",
                        column: x => x.ClaimId,
                        principalTable: "Claims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Amount", "ClaimNumber", "CreatedAt", "MemberName", "ProviderName", "ServiceDate", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 15000m, "CLM1001", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ravi Kumar", "Apollo Hospital", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 23000m, "CLM1002", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Priya Sharma", "Fortis Hospital", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Under Review", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 5000m, "CLM1003", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Arun Singh", "AIIMS", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 12000m, "CLM1004", new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Meena Patel", "Max Healthcare", new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejected", new DateTime(2025, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 17500m, "CLM1005", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Suresh Reddy", "Narayana Hospital", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", new DateTime(2025, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 8900m, "CLM1006", new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anita Verma", "Care Hospital", new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2025, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 21000m, "CLM1007", new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rahul Nair", "Manipal Hospital", new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Under Review", new DateTime(2025, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 9500m, "CLM1008", new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kavita Joshi", "Medanta Hospital", new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Submitted", new DateTime(2025, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 30000m, "CLM1009", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deepak Gupta", "Global Hospital", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 14000m, "CLM1010", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lakshmi Narayan", "Sunrise Hospital", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Under Review", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimNotes_ClaimId",
                table: "ClaimNotes",
                column: "ClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ClaimNumber",
                table: "Claims",
                column: "ClaimNumber",
                unique: true,
                filter: "[ClaimNumber] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimNotes");

            migrationBuilder.DropTable(
                name: "Claims");
        }
    }
}
