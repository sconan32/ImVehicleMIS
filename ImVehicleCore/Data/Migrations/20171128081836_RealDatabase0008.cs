using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class RealDatabase0008 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoDriver",
                table: "Vehicles",
                newName: "PhotoInsuarance");

            migrationBuilder.RenameColumn(
                name: "LastRegisterDate",
                table: "Vehicles",
                newName: "YearlyAuditDate");

            migrationBuilder.RenameColumn(
                name: "ChiefCaption",
                table: "Groups",
                newName: "ChiefTitle");

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoAudit",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealOwner",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterDate",
                table: "Vehicles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Usage",
                table: "Vehicles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentFilePath",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FirstLicenseIssueDate",
                table: "Drivers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LivingAddress",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoAvatar",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarrantyCode",
                table: "Drivers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientPath = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    GroupId = table.Column<int>(nullable: true),
                    GroupId1 = table.Column<long>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ServerPath = table.Column<string>(nullable: true),
                    Size = table.Column<long>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_GroupId",
                table: "Drivers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_GroupId1",
                table: "Files",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Groups_GroupId",
                table: "Drivers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Groups_GroupId",
                table: "Drivers");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_GroupId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoAudit",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RealOwner",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "RegisterDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Usage",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AttachmentFilePath",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "FirstLicenseIssueDate",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "LivingAddress",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "PhotoAvatar",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "WarrantyCode",
                table: "Drivers");

            migrationBuilder.RenameColumn(
                name: "YearlyAuditDate",
                table: "Vehicles",
                newName: "LastRegisterDate");

            migrationBuilder.RenameColumn(
                name: "PhotoInsuarance",
                table: "Vehicles",
                newName: "PhotoDriver");

            migrationBuilder.RenameColumn(
                name: "ChiefTitle",
                table: "Groups",
                newName: "ChiefCaption");
        }
    }
}
