using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Donnum.DonorService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedNewDonorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Donors",
                columns: new[] { "Id", "Age", "AuthUserId", "BloodGroup", "City", "CreatedAt", "CreatedBy", "FirstName", "Gender", "HasGuardianAuthorization", "HasMedicalRestriction", "HasRecentTattooOrPiercing", "IsDeleted", "IsHealthy", "IsPregnant", "LastName", "ModifiedBy", "Phone", "PhoneNumber", "Points", "Province", "RhFactor", "Street", "UpdatedAt", "WeightKg", "Location_Latitude", "Location_Longitude" },
                values: new object[] { new Guid("d0000000-d000-d000-d000-d00000000001"), 25, new Guid("f9faea36-22f2-42dc-934c-fc5910baa012"), "A", "Buenos Aires", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Test", 2, true, false, false, false, true, false, "Donor", null, null, "+5491155550102", 50, "Buenos Aires", "Positive", "Av. Santa Fe 1234", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 65m, -34.6137m, -58.3916m });

            migrationBuilder.InsertData(
                table: "Donations",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DonationDate", "DonationRequestId", "DonorId", "IsDeleted", "MedicalCenterId", "ModifiedBy" },
                values: new object[] { new Guid("e0000000-e000-e000-e000-e00000000001"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("11111111-1111-1111-1111-111111111111"), new Guid("d0000000-d000-d000-d000-d00000000001"), false, new Guid("22222222-2222-2222-2222-222222222222"), null });

            migrationBuilder.InsertData(
                table: "DonorBadges",
                columns: new[] { "Id", "AssignedAt", "BadgeId", "CreatedBy", "DonorId", "IsDeleted", "ModifiedBy" },
                values: new object[] { new Guid("b0000000-b000-b000-b000-b00000000001"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("33333333-3333-3333-3333-333333333333"), null, new Guid("d0000000-d000-d000-d000-d00000000001"), false, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Donations",
                keyColumn: "Id",
                keyValue: new Guid("e0000000-e000-e000-e000-e00000000001"));

            migrationBuilder.DeleteData(
                table: "DonorBadges",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-b000-b000-b000-b00000000001"));

            migrationBuilder.DeleteData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: new Guid("d0000000-d000-d000-d000-d00000000001"));
        }
    }
}
