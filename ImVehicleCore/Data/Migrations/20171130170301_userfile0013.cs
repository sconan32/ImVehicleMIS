using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class userfile0013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Depart",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Depart",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RealName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "AspNetUsers");
        }
    }
}
