using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class PunchClockAMPM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditorTimeIn",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "TimeOut",
                table: "Attendance",
                newName: "TimeOutPM");

            migrationBuilder.RenameColumn(
                name: "TimeIn",
                table: "Attendance",
                newName: "TimeOutAM");

            migrationBuilder.RenameColumn(
                name: "EditorTimeOut",
                table: "Attendance",
                newName: "Editor");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeInAM",
                table: "Attendance",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeInPM",
                table: "Attendance",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeInAM",
                table: "Attendance");

            migrationBuilder.DropColumn(
                name: "TimeInPM",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "TimeOutPM",
                table: "Attendance",
                newName: "TimeOut");

            migrationBuilder.RenameColumn(
                name: "TimeOutAM",
                table: "Attendance",
                newName: "TimeIn");

            migrationBuilder.RenameColumn(
                name: "Editor",
                table: "Attendance",
                newName: "EditorTimeOut");

            migrationBuilder.AddColumn<string>(
                name: "EditorTimeIn",
                table: "Attendance",
                nullable: true);
        }
    }
}
