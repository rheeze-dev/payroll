using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class EmployeesUpdate4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryLedger",
                table: "SalaryLedger");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "SalaryLedger",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdNumber",
                table: "SalaryLedger",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryLedger",
                table: "SalaryLedger",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryLedger",
                table: "SalaryLedger");

            migrationBuilder.AlterColumn<string>(
                name: "IdNumber",
                table: "SalaryLedger",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "SalaryLedger",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryLedger",
                table: "SalaryLedger",
                column: "IdNumber");
        }
    }
}
