using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedCola2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalBasicPay",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalBasicPay",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalBasicPay",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalBasicPay",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "TotalBasicPay",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TotalBasicPay",
                table: "CurrentLedger");
        }
    }
}
