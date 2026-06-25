using Donnum.DonorService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Donnum.DonorService.Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260625010100_AddDonorContact")]
public sealed class AddDonorContact : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "FirstName",
            table: "Donors",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "LastName",
            table: "Donors",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            table: "Donors",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "");

        migrationBuilder.UpdateData(
            table: "Donors",
            keyColumn: "Id",
            keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
            columns: ["FirstName", "LastName", "PhoneNumber"],
            values: ["Donante", "Demo", "+5491155550101"]);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "FirstName", table: "Donors");
        migrationBuilder.DropColumn(name: "LastName", table: "Donors");
        migrationBuilder.DropColumn(name: "PhoneNumber", table: "Donors");
    }
}
