using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class UpdateAttendance11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "AmountOT",
                table: "SalaryLedger",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfMinSunday",
                table: "Attendance",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfMinSunday",
                table: "Attendance");

            migrationBuilder.AlterColumn<int>(
                name: "AmountOT",
                table: "SalaryLedger",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
