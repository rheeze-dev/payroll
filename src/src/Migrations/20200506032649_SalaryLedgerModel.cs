using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace src.Migrations
{
    public partial class SalaryLedgerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryLedger",
                columns: table => new
                {
                    IdNumber = table.Column<string>(nullable: false),
                    AddAdjustment = table.Column<int>(nullable: false),
                    AmountOT = table.Column<int>(nullable: false),
                    AmountRH = table.Column<int>(nullable: false),
                    AmountSH = table.Column<int>(nullable: false),
                    AmountSundays = table.Column<int>(nullable: false),
                    AmountTardiness = table.Column<int>(nullable: false),
                    BasicPay = table.Column<int>(nullable: false),
                    CashOut = table.Column<int>(nullable: false),
                    Charges1 = table.Column<int>(nullable: false),
                    Charges2 = table.Column<int>(nullable: false),
                    CreateAt = table.Column<DateTime>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    DateTimeEdited = table.Column<DateTime>(nullable: false),
                    DaysOfWorkBP = table.Column<int>(nullable: false),
                    Editor = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(maxLength: 100, nullable: true),
                    GrossPay = table.Column<int>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    LessAdjustment = table.Column<int>(nullable: false),
                    LoanAmount = table.Column<int>(nullable: false),
                    LoanBalance = table.Column<int>(nullable: false),
                    NetAmountPaid = table.Column<int>(nullable: false),
                    NumberOfDaysRH = table.Column<int>(nullable: false),
                    NumberOfHrsSH = table.Column<int>(nullable: false),
                    NumberOfMinOt = table.Column<int>(nullable: false),
                    NumberOfMinSundays = table.Column<int>(nullable: false),
                    NumberOfMinTardiness = table.Column<int>(nullable: false),
                    PaymentPlan = table.Column<string>(nullable: true),
                    SalaryLoan = table.Column<int>(nullable: false),
                    TotalAmountBP = table.Column<int>(nullable: false),
                    TotalDeductions = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryLedger", x => x.IdNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryLedger");
        }
    }
}
