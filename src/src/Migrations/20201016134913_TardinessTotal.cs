using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TardinessTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfMinTardiness",
                table: "Attendance",
                newName: "TotalNumberOfMinTardiness");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMinTardinessAM",
                table: "Attendance",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMinTardinessPM",
                table: "Attendance",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfMinTardinessAM",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "NumberOfMinTardinessPM",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "TotalNumberOfMinTardiness",
                table: "Attendance",
                newName: "NumberOfMinTardiness");
        }
    }
}
