using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class foreignkey0005 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Towns_TownId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Towns_TownId",
                table: "Groups");

            migrationBuilder.AlterColumn<long>(
                name: "TownId",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "TownId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Towns_TownId",
                table: "AspNetUsers",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Towns_TownId",
                table: "Groups",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Towns_TownId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Towns_TownId",
                table: "Groups");

            migrationBuilder.AlterColumn<long>(
                name: "TownId",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TownId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

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
        }
    }
}
