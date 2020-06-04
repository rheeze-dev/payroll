using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class AddedCola : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cola",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cola",
                table: "Employees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cola",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cola",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "Cola",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Cola",
                table: "CurrentLedger");
        }
    }
}
