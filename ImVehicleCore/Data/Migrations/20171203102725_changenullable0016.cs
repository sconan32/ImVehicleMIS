using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class changenullable0016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "Vehicles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "YearlyAuditDate",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProductionDate",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsuranceExpiredDate",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstRegisterDate",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRegisterDate",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LicenseValidYears",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LicenseIssueDate",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "FirstLicenseIssueDate",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstRegisterDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LastRegisterDate",
                table: "Vehicles");

            migrationBuilder.AlterColumn<DateTime>(
                name: "YearlyAuditDate",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ProductionDate",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InsuranceExpiredDate",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "Vehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "LicenseValidYears",
                table: "Drivers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LicenseIssueDate",
                table: "Drivers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "FirstLicenseIssueDate",
                table: "Drivers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
