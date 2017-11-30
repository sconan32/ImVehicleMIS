using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class drivertownfk0010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<long>(
                name: "TownId",
                table: "Drivers",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Visible",
                table: "AspNetRoles",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_TownId",
                table: "Drivers",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drivers_Towns_TownId",
                table: "Drivers",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Towns_TownId",
                table: "Drivers");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_TownId",
                table: "Drivers");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "Drivers");

            migrationBuilder.AlterColumn<bool>(
                name: "Visible",
                table: "AspNetRoles",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: "");
        }
    }
}
