using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Attendance6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeIn",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TimeOut",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "TotalTimeIn",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTimeOut",
                table: "Employees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTimeIn",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TotalTimeOut",
                table: "Employees");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeIn",
                table: "Employees",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOut",
                table: "Employees",
                nullable: true);
        }
    }
}
