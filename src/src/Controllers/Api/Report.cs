using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using OfficeOpenXml;
using src.Data;
using src.Models;
using src.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace src.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Report")]
    //[Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public ReportController(ApplicationDbContext context,
            IDotnetdesk dotnetdesk,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _dotnetdesk = dotnetdesk;
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        [HttpGet("PayslipReport")]
        public async Task<IActionResult> PayslipReport(int Year, int Month)
        {
            // query data from database  
            await Task.Yield();

            var payslip = _context.SalaryLedger.Where(x => x.DateAndTime.Value.Year.Equals(Year) && x.DateAndTime.Value.Month.Equals(Month)).Select(y => new { Date = y.DateAndTime.Value.ToString("MMMM yyyy"), Fullname = y.FullName, BasicPay = y.BasicPay, DaysOfWork = y.DaysOfWorkBP, TotalRegularWage = y.TotalAmountBP, OvertimeMinutes = y.NumberOfMinOT, Overtime = y.AmountOT, SundayMinutes = y.NumberOfMinSundays, Sunday = y.AmountSundays, HolidayPay = y.AmountSH + y.AmountRH, Others = y.AddAdjustment, Grosspay = y.GrossPay, Charges = y.Charges1, TardinessMinutes = y.NumberOfMinTardiness, Tardiness = y.AmountTardiness, Cashout = y.CashOut, SalaryLoanAmount = y.LoanAmount, Pagibig = y.PagibigEmployee, Philhealth = y.PhilHealthEmployee, SSS = y.SSSEmployee, TotalDeductions = y.TotalDeductions, NetAmountPaid = y.NetAmountPaid }).ToList();

            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(payslip, true);
                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Salary ledger and Payslip {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("PayslipReportDate")]
        public async Task<IActionResult> PayslipReportDate(int Date)
        {
            // query data from database  
            await Task.Yield();

            var stream = new MemoryStream();

            if (Date == 1000000)
            {
                {
                    var all = _context.SalaryLedger.Select(y => new { Date = y.DateAndTime.Value.ToString("MMMM yyyy"), Fullname = y.FullName, BasicPay = y.BasicPay, DaysOfWork = y.DaysOfWorkBP, TotalRegularWage = y.TotalAmountBP, OvertimeMinutes = y.NumberOfMinOT, Overtime = y.AmountOT, SundayMinutes = y.NumberOfMinSundays, Sunday = y.AmountSundays, HolidayPay = y.AmountSH + y.AmountRH, Others = y.AddAdjustment, Grosspay = y.GrossPay, Charges = y.Charges1, TardinessMinutes = y.NumberOfMinTardiness, Tardiness = y.AmountTardiness, Cashout = y.CashOut, SalaryLoanAmount = y.LoanAmount, Pagibig = y.PagibigEmployee, Philhealth = y.PhilHealthEmployee, SSS = y.SSSEmployee, TotalDeductions = y.TotalDeductions, NetAmountPaid = y.NetAmountPaid }).ToList();
                    using (var package = new ExcelPackage(stream))
                    {
                        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                        workSheet.Cells.LoadFromCollection(all, true);
                        package.Save();
                    }
                }
            }
            else if (Date == 1)
            {
                var lastMonth = _context.SalaryLedger.Where(x => x.DateAndTime >= DateTime.Today).Select(y => new { Date = y.DateAndTime.Value.ToString("MMMM yyyy"), Fullname = y.FullName, BasicPay = y.BasicPay, DaysOfWork = y.DaysOfWorkBP, TotalRegularWage = y.TotalAmountBP, OvertimeMinutes = y.NumberOfMinOT, Overtime = y.AmountOT, SundayMinutes = y.NumberOfMinSundays, Sunday = y.AmountSundays, HolidayPay = y.AmountSH + y.AmountRH, Others = y.AddAdjustment, Grosspay = y.GrossPay, Charges = y.Charges1, TardinessMinutes = y.NumberOfMinTardiness, Tardiness = y.AmountTardiness, Cashout = y.CashOut, SalaryLoanAmount = y.LoanAmount, Pagibig = y.PagibigEmployee, Philhealth = y.PhilHealthEmployee, SSS = y.SSSEmployee, TotalDeductions = y.TotalDeductions, NetAmountPaid = y.NetAmountPaid }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 31)
            {
                var lastMonth = _context.SalaryLedger.Where(x => x.DateAndTime >= DateTime.Today.AddDays(-31)).Select(y => new { Date = y.DateAndTime.Value.ToString("MMMM yyyy"), Fullname = y.FullName, BasicPay = y.BasicPay, DaysOfWork = y.DaysOfWorkBP, TotalRegularWage = y.TotalAmountBP, OvertimeMinutes = y.NumberOfMinOT, Overtime = y.AmountOT, SundayMinutes = y.NumberOfMinSundays, Sunday = y.AmountSundays, HolidayPay = y.AmountSH + y.AmountRH, Others = y.AddAdjustment, Grosspay = y.GrossPay, Charges = y.Charges1, TardinessMinutes = y.NumberOfMinTardiness, Tardiness = y.AmountTardiness, Cashout = y.CashOut, SalaryLoanAmount = y.LoanAmount, Pagibig = y.PagibigEmployee, Philhealth = y.PhilHealthEmployee, SSS = y.SSSEmployee, TotalDeductions = y.TotalDeductions, NetAmountPaid = y.NetAmountPaid }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastMonth, true);
                    package.Save();
                }
            }
            else if (Date == 365)
            {
                var lastYear = _context.SalaryLedger.Where(x => x.DateAndTime >= DateTime.Today.AddDays(-365)).Select(y => new { Date = y.DateAndTime.Value.ToString("MMMM yyyy"), Fullname = y.FullName, BasicPay = y.BasicPay, DaysOfWork = y.DaysOfWorkBP, TotalRegularWage = y.TotalAmountBP, OvertimeMinutes = y.NumberOfMinOT, Overtime = y.AmountOT, SundayMinutes = y.NumberOfMinSundays, Sunday = y.AmountSundays, HolidayPay = y.AmountSH + y.AmountRH, Others = y.AddAdjustment, Grosspay = y.GrossPay, Charges = y.Charges1, TardinessMinutes = y.NumberOfMinTardiness, Tardiness = y.AmountTardiness, Cashout = y.CashOut, SalaryLoanAmount = y.LoanAmount, Pagibig = y.PagibigEmployee, Philhealth = y.PhilHealthEmployee, SSS = y.SSSEmployee, TotalDeductions = y.TotalDeductions, NetAmountPaid = y.NetAmountPaid }).ToList();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(lastYear, true);
                    package.Save();
                }
            }

            stream.Position = 0;
            string excelName = $"Salary ledger and Payslip {DateTime.Now.ToString("MMMM-dd-yyyy")}.xlsx";

            //return File(stream, "application/octet-stream", excelName);  
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

    }
}