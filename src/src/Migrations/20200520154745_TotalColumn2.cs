using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class TotalColumn2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PagibigTotal",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "PhilhealthTotal",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "SSSTotal",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "PagibigTotal",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "PhilhealthTotal",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "SSSTotal",
                table: "CurrentLedger");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PagibigTotal",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhilhealthTotal",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SSSTotal",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PagibigTotal",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhilhealthTotal",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SSSTotal",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
