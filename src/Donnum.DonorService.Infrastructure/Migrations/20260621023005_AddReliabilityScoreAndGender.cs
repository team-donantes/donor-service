using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Donnum.DonorService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReliabilityScoreAndGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Donors_DonorId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "ReliabilityScore",
                table: "Donors");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Donors",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Donors",
                newName: "Location_Latitude");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Donors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Donors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Donors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Donors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DonorBadges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DonorBadges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "DonorBadges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Donations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Donations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "DonationRequestParticipations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DonationRequestParticipations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "DonationRequestParticipations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Badges",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Badges",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ReliabilityScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DonorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    LastCalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReliabilityScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReliabilityScores_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "CreatedBy", "IsDeleted", "ModifiedBy" },
                values: new object[] { null, false, null });

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                columns: new[] { "CreatedBy", "IsDeleted", "ModifiedBy" },
                values: new object[] { null, false, null });

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"),
                columns: new[] { "CreatedBy", "IsDeleted", "ModifiedBy" },
                values: new object[] { null, false, null });

            migrationBuilder.UpdateData(
                table: "Badges",
                keyColumn: "Id",
                keyValue: new Guid("66666666-6666-6666-6666-666666666666"),
                columns: new[] { "CreatedBy", "IsDeleted", "ModifiedBy" },
                values: new object[] { null, false, null });

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "CreatedBy", "Gender", "IsDeleted", "ModifiedBy", "Points" },
                values: new object[] { null, 1, false, null, 0 });

            migrationBuilder.InsertData(
                table: "ReliabilityScores",
                columns: new[] { "Id", "CreatedBy", "DonorId", "IsDeleted", "LastCalculatedAt", "ModifiedBy", "Score" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), null, new Guid("11111111-1111-1111-1111-111111111111"), false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 100 });

            migrationBuilder.CreateIndex(
                name: "IX_ReliabilityScores_DonorId",
                table: "ReliabilityScores",
                column: "DonorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Donors_DonorId",
                table: "Donations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Donors_DonorId",
                table: "Donations");

            migrationBuilder.DropTable(
                name: "ReliabilityScores");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Donors");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DonorBadges");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DonorBadges");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "DonorBadges");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DonationRequestParticipations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DonationRequestParticipations");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "DonationRequestParticipations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Badges");

            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Donors",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Donors",
                newName: "Latitude");

            migrationBuilder.AddColumn<int>(
                name: "ReliabilityScore",
                table: "Donors",
                type: "int",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Donors",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ReliabilityScore",
                value: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Donors_DonorId",
                table: "Donations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
