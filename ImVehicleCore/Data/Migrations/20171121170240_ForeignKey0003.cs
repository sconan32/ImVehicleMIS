using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class ForeignKey0003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Towns_TownItemId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_Groups_SecurityItemId",
                table: "SecurityPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_Towns_Districts_DistrictId1",
                table: "Towns");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Towns_DistrictId1",
                table: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_SecurityPersons_SecurityItemId",
                table: "SecurityPersons");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TownItemId",
                table: "Groups");

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
                name: "DistrictId",
                table: "Towns");

            migrationBuilder.DropColumn(
                name: "DistrictId1",
                table: "Towns");

            migrationBuilder.DropColumn(
                name: "SecurityItemId",
                table: "SecurityPersons");

            migrationBuilder.DropColumn(
                name: "PhotoMainPath",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PhotoSecurityPath",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TownItemId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PhotoDriverLicensePath",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoIdCard1Path",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoIdCard2Path",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoWarrantyPath",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "PhotoWarrantyPath",
                table: "Groups",
                newName: "ChiefTel");

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DriverId",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoDriver",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoFront",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoRear",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DirstrictId",
                table: "Towns",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "SecurityPersons",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoMain",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoSecurity",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoWarranty",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TownId",
                table: "Groups",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoDriverLicense",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoIdCard1",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoIdCard2",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoWarranty",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TownId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Towns_DirstrictId",
                table: "Towns",
                column: "DirstrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPersons_GroupId",
                table: "SecurityPersons",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TownId",
                table: "Groups",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TownId",
                table: "AspNetUsers",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Towns_TownId",
                table: "AspNetUsers",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Towns_TownId",
                table: "Groups",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_Groups_GroupId",
                table: "SecurityPersons",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Towns_Districts_DirstrictId",
                table: "Towns",
                column: "DirstrictId",
                principalTable: "Districts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                table: "Vehicles",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Towns_TownId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Towns_TownId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_Groups_GroupId",
                table: "SecurityPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_Towns_Districts_DirstrictId",
                table: "Towns");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Drivers_DriverId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Groups_GroupId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Towns_DirstrictId",
                table: "Towns");

            migrationBuilder.DropIndex(
                name: "IX_SecurityPersons_GroupId",
                table: "SecurityPersons");

            migrationBuilder.DropIndex(
                name: "IX_Groups_TownId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TownId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoDriver",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoFront",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoRear",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DirstrictId",
                table: "Towns");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "SecurityPersons");

            migrationBuilder.DropColumn(
                name: "PhotoMain",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PhotoSecurity",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PhotoWarranty",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "PhotoDriverLicense",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoIdCard1",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoIdCard2",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoWarranty",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ChiefTel",
                table: "Groups",
                newName: "PhotoWarrantyPath");

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "DriverId",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(long));

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

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Towns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "DistrictId1",
                table: "Towns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SecurityItemId",
                table: "SecurityPersons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoMainPath",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoSecurityPath",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TownItemId",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoDriverLicensePath",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoIdCard1Path",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoIdCard2Path",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoWarrantyPath",
                table: "Drivers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Towns_DistrictId1",
                table: "Towns",
                column: "DistrictId1");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPersons_SecurityItemId",
                table: "SecurityPersons",
                column: "SecurityItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_TownItemId",
                table: "Groups",
                column: "TownItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Towns_TownItemId",
                table: "Groups",
                column: "TownItemId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_Groups_SecurityItemId",
                table: "SecurityPersons",
                column: "SecurityItemId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Towns_Districts_DistrictId1",
                table: "Towns",
                column: "DistrictId1",
                principalTable: "Districts",
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
    }
}
