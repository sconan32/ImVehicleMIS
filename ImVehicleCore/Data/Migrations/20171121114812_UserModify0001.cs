using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class UserModify0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Districts",
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
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Newses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Excerpt = table.Column<string>(nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: false),
                    HasDateRange = table.Column<bool>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    PublishDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Towns",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DistrictId = table.Column<int>(nullable: false),
                    DistrictId1 = table.Column<long>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Towns_Districts_DistrictId1",
                        column: x => x.DistrictId1,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "SecurityPersons",
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
                    SecurityItemId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Tel = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityPersons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityPersons_SecurityItems_SecurityItemId",
                        column: x => x.SecurityItemId,
                        principalTable: "SecurityItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateBy = table.Column<long>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    DriverName = table.Column<string>(nullable: true),
                    LicenceNumber = table.Column<string>(nullable: true),
                    Metadata = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: false),
                    ModifyBy = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SecurityItemId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    VersionNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_SecurityItems_SecurityItemId",
                        column: x => x.SecurityItemId,
                        principalTable: "SecurityItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityItems_TownItemId",
                table: "SecurityItems",
                column: "TownItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPersons_SecurityItemId",
                table: "SecurityPersons",
                column: "SecurityItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Towns_DistrictId1",
                table: "Towns",
                column: "DistrictId1");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_SecurityItemId",
                table: "Vehicles",
                column: "SecurityItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Newses");

            migrationBuilder.DropTable(
                name: "SecurityPersons");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "SecurityItems");

            migrationBuilder.DropTable(
                name: "Towns");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
