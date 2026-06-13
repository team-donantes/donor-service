using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Donnum.DonorService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BadgeType = table.Column<int>(type: "int", nullable: false),
                    BadgePoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BloodGroup = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    RhFactor = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Province = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: false),
                    ReliabilityScore = table.Column<int>(type: "int", precision: 5, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DonationRequestParticipations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonationRequestParticipations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonationRequestParticipations_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MedicalCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donations_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DonorBadges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BadgeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonorBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DonorBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DonorBadges_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "BadgePoints", "BadgeType", "Name" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), 50, 2, "Donante Donor" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 100, 3, "Frecuente" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 250, 4, "Comprometido" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 500, 5, "Heroe" }
                });

            migrationBuilder.InsertData(
                table: "Donors",
                columns: new[] { "Id", "AuthUserId", "BloodGroup", "City", "CreatedAt", "Latitude", "Longitude", "Province", "ReliabilityScore", "RhFactor", "Street", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222"), "O", "Buenos Aires", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), -34.6037m, -58.3816m, "Buenos Aires", 100, "Positive", "Av. Corrientes 1234", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_BadgeType",
                table: "Badges",
                column: "BadgeType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DonationRequestParticipations_DonorId_DonationRequestId",
                table: "DonationRequestParticipations",
                columns: new[] { "DonorId", "DonationRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonationRequestId",
                table: "Donations",
                column: "DonationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorId",
                table: "Donations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_MedicalCenterId",
                table: "Donations",
                column: "MedicalCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorBadges_BadgeId",
                table: "DonorBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_DonorBadges_DonorId_BadgeId",
                table: "DonorBadges",
                columns: new[] { "DonorId", "BadgeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donors_AuthUserId",
                table: "Donors",
                column: "AuthUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DonationRequestParticipations");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "DonorBadges");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Donors");
        }
    }
}
