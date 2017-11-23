using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class AddDriver0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_SecurityItems_SecurityItemId",
                table: "SecurityPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_SecurityItems_SecurityItemId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "SecurityItems");

            migrationBuilder.RenameColumn(
                name: "SecurityItemId",
                table: "Vehicles",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_SecurityItemId",
                table: "Vehicles",
                newName: "IX_Vehicles_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DriverId",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverTel",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceExpiredDate",
                table: "Vehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastRegisterDate",
                table: "Vehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PhotoDriverPath",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoFrontPath",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoRearPath",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductionDate",
                table: "Vehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VehicleStatus",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    IdCardNumber = table.Column<string>(nullable: true),
                    LicenseIssueDate = table.Column<DateTime>(nullable: false),
                    LicenseType = table.Column<int>(nullable: false),
                    LicenseValidYears = table.Column<int>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PhotoDriverLicensePath = table.Column<string>(nullable: true),
                    PhotoIdCard1Path = table.Column<string>(nullable: true),
                    PhotoIdCard2Path = table.Column<string>(nullable: true),
                    PhotoWarrantyPath = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tel = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    ChiefName = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PhotoMainPath = table.Column<string>(nullable: true),
                    PhotoSecurityPath = table.Column<string>(nullable: true),
                    PhotoWarrantyPath = table.Column<string>(nullable: true),
                    RegisterAddress = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TownItemId = table.Column<long>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Towns_TownItemId",
                        column: x => x.TownItemId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TownItemId",
                table: "Groups",
                column: "TownItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_Groups_SecurityItemId",
                table: "SecurityPersons",
                column: "SecurityItemId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                table: "Vehicles",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_Groups_SecurityItemId",
                table: "SecurityPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DriverTel",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "InsuranceExpiredDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "LastRegisterDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoDriverPath",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoFrontPath",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoRearPath",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ProductionDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleStatus",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Vehicles",
                newName: "SecurityItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_GroupId",
                table: "Vehicles",
                newName: "IX_Vehicles_SecurityItemId");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "SecurityItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    RegisterAddress = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TownItemId = table.Column<long>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityItems_Towns_TownItemId",
                        column: x => x.TownItemId,
                        principalTable: "Towns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityItems_TownItemId",
                table: "SecurityItems",
                column: "TownItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_SecurityItems_SecurityItemId",
                table: "SecurityPersons",
                column: "SecurityItemId",
                principalTable: "SecurityItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_SecurityItems_SecurityItemId",
                table: "Vehicles",
                column: "SecurityItemId",
                principalTable: "SecurityItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
