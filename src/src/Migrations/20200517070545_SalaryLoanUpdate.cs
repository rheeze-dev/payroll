using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class SalaryLoanUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SalaryLoanChecker",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SalaryLoanChecker",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryLoanChecker",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "SalaryLoanChecker",
                table: "CurrentLedger");
        }
    }
}
