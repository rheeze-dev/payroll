using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class CurrentLedgerDb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentLedger",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AddAdjustment = table.Column<int>(nullable: false),
                    AmountOT = table.Column<double>(nullable: false),
                    AmountRH = table.Column<double>(nullable: false),
                    AmountSH = table.Column<double>(nullable: false),
                    AmountSundays = table.Column<double>(nullable: false),
                    AmountTardiness = table.Column<double>(nullable: false),
                    BasicPay = table.Column<int>(nullable: false),
                    CashOut = table.Column<int>(nullable: false),
                    Charges1 = table.Column<int>(nullable: false),
                    Charges2 = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    DateAndTime = table.Column<DateTime>(nullable: true),
                    DaysOfWorkBP = table.Column<int>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    GrossPay = table.Column<double>(nullable: false),
                    IdNumber = table.Column<string>(nullable: true),
                    LessAdjustment = table.Column<int>(nullable: false),
                    LoanAmount = table.Column<double>(nullable: false),
                    LoanBalance = table.Column<double>(nullable: false),
                    NetAmountPaid = table.Column<double>(nullable: false),
                    NumberOfDaysRH = table.Column<int>(nullable: false),
                    NumberOfHrsSH = table.Column<int>(nullable: false),
                    NumberOfMinOT = table.Column<int>(nullable: false),
                    NumberOfMinSundays = table.Column<int>(nullable: false),
                    NumberOfMinTardiness = table.Column<int>(nullable: false),
                    PaymentPlan = table.Column<string>(nullable: true),
                    SalaryLoan = table.Column<int>(nullable: false),
                    TotalAmountBP = table.Column<double>(nullable: false),
                    TotalDeductions = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentLedger", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentLedger");
        }
    }
}
