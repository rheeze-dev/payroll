using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class Benefits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PagibigEmployee",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PagibigEmployer",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhilHealthEmployee",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhilHealthEmployer",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SSSEmployee",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SSSEmployer",
                table: "SalaryLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PagibigEmployee",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PagibigEmployer",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhilHealthEmployee",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhilHealthEmployer",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SSSEmployee",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SSSEmployer",
                table: "CurrentLedger",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PagibigEmployee",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "PagibigEmployer",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "PhilHealthEmployee",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "PhilHealthEmployer",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "SSSEmployee",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "SSSEmployer",
                table: "SalaryLedger");

            migrationBuilder.DropColumn(
                name: "PagibigEmployee",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "PagibigEmployer",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "PhilHealthEmployee",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "PhilHealthEmployer",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "SSSEmployee",
                table: "CurrentLedger");

            migrationBuilder.DropColumn(
                name: "SSSEmployer",
                table: "CurrentLedger");
        }
    }
}
