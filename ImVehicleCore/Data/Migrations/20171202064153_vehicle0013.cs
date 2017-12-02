using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class vehicle0013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TownId",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TownId",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseRoleId",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TownId",
                table: "Vehicles",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_TownId",
                table: "Files",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_BaseRoleId",
                table: "AspNetRoles",
                column: "BaseRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoles_AspNetRoles_BaseRoleId",
                table: "AspNetRoles",
                column: "BaseRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Towns_TownId",
                table: "Files",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Towns_TownId",
                table: "Vehicles",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoles_AspNetRoles_BaseRoleId",
                table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Files_Towns_TownId",
                table: "Files");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Towns_TownId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_TownId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Files_TownId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoles_BaseRoleId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BaseRoleId",
                table: "AspNetRoles");
        }
    }
}
