using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Attendance2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeEdited",
                table: "Attendance",
                newName: "TimeOut");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeIn",
                table: "Attendance",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeIn",
                table: "Attendance");

            migrationBuilder.RenameColumn(
                name: "TimeOut",
                table: "Attendance",
                newName: "DateTimeEdited");
        }
    }
}
