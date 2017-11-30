using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class modifysperson0012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_Groups_GroupId",
                table: "SecurityPersons");

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Vehicles",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Towns",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Towns",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "SecurityPersons",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                table: "SecurityPersons",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "SecurityPersons",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "SecurityPersons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdCardNum",
                table: "SecurityPersons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SecurityPersons",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TownId",
                table: "SecurityPersons",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Newses",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Newses",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Groups",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Files",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Files",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "ModifyBy",
                table: "Districts",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "CreateBy",
                table: "Districts",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_SecurityPersons_TownId",
                table: "SecurityPersons",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_Groups_GroupId",
                table: "SecurityPersons",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_Towns_TownId",
                table: "SecurityPersons",
                column: "TownId",
                principalTable: "Towns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_Groups_GroupId",
                table: "SecurityPersons");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityPersons_Towns_TownId",
                table: "SecurityPersons");

            migrationBuilder.DropIndex(
                name: "IX_SecurityPersons_TownId",
                table: "SecurityPersons");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "SecurityPersons");

            migrationBuilder.DropColumn(
                name: "IdCardNum",
                table: "SecurityPersons");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SecurityPersons");

            migrationBuilder.DropColumn(
                name: "TownId",
                table: "SecurityPersons");

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Vehicles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Towns",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Towns",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "SecurityPersons",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                table: "SecurityPersons",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "SecurityPersons",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Newses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Newses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Groups",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Files",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Files",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Drivers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Drivers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ModifyBy",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CreateBy",
                table: "Districts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityPersons_Groups_GroupId",
                table: "SecurityPersons",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
