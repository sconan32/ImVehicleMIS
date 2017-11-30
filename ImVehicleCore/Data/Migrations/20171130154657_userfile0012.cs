using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ImVehicleCore.Data.Migrations
{
    public partial class userfile0012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Groups_GroupId1",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_GroupId1",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Files");

            migrationBuilder.AlterColumn<long>(
                name: "GroupId",
                table: "Files",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Files",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DownloadCount",
                table: "Files",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Files_GroupId",
                table: "Files",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Groups_GroupId",
                table: "Files",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Groups_GroupId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_GroupId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "Files");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Files",
                nullable: true,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GroupId1",
                table: "Files",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Files_GroupId1",
                table: "Files",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Groups_GroupId1",
                table: "Files",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
