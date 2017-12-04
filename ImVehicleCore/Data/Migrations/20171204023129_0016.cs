using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class _0016 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DumpDate",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PhotoGps",
                table: "Vehicles",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Newses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Newses",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DumpDate",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "PhotoGps",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Newses",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Newses",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
