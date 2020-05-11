using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using src.Data;
using src.Models;
using src.Services;

namespace src.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/TimeClock")]
    //[Authorize]
    public class TimeClockController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public TimeClockController(ApplicationDbContext context,
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

        // GET: api/TimeClock/GetTimeClock
        [HttpGet("{organizationId}")]
        public IActionResult GetTimeClock([FromRoute]Guid organizationId)
        {
            var employees = _context.Employees.ToList();
            return Json(new { data = employees });
        }

        //POST: api/TimeClock/PostTimeIn
        [HttpPost("PostTimeIn")]
        public async Task<IActionResult> PostTimeIn(string idNumber)
        {
            Employees employees = _context.Employees.Where(x => x.IdNumber == idNumber).FirstOrDefault();
            SalaryLedger currentSalaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == idNumber).OrderByDescending(x => x.DateAndTime).First();
            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == idNumber).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);
            var totalTimeIn = employees.TotalTimeIn;
            employees.Email = employees.Email;
            employees.TotalTimeIn = totalTimeIn + 1;

            if (employees.BasicPay == null)
            {
                return Json(new { success = false, message = "Enter basic pay first!" });
            }

            //var checker = employees.DateTimeChecker.Value.AddHours(1);
            //if (checker > DateTime.Now)
            //{
            //    return Json(new { success = false, message = "Minumum time is 1 hour!" });
            //}
            //else
            //{
            //    employees.DateTimeChecker = DateTime.Now;
            //    _context.Employees.Update(employees);
            //}

            Attendance attendance = new Attendance
            {
                IdNumber = idNumber,
                FullName = employees.FullName,
                TimeIn = DateTime.Now,
                EditorTimeIn = info.FullName,
            };

            var tardiness = DateTime.Now;
            tardiness = new DateTime(tardiness.Year, tardiness.Month, tardiness.Day, 08, 00, 00);
            TimeSpan solve = attendance.TimeIn.Value - tardiness;
            int tardinessMin = (int)solve.TotalMinutes;

            if (attendance.TimeIn.Value > tardiness)
            {
                attendance.NumberOfMinTardiness = tardinessMin;
            }

            attendance.Id = Guid.NewGuid();
            _context.Attendance.Add(attendance);

            var monthNow = DateTime.Now.Month;
            //var nextMonth = monthNow + 1;

            SalaryLedger salaryLedger = new SalaryLedger
            {
                IdNumber = idNumber,
                FullName = currentSalaryLedger.FullName,
                Email = currentSalaryLedger.Email,
                BasicPay = currentSalaryLedger.BasicPay
            };

            var day = DateTime.Now.Day;
            if (day >= 16)
            {
                currentLedger.MidMonth = true;
                //salaryLedger.MidMonth = true;
            }
            else
            {
                currentLedger.MidMonth = false;
                //salaryLedger.MidMonth = false;
            }

            var month = currentSalaryLedger.DateAndTime.Value.Month;

            if (month != monthNow || currentSalaryLedger.MidMonth != currentLedger.MidMonth)
            {
                salaryLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness + attendance.NumberOfMinTardiness;
                salaryLedger.Id = Guid.NewGuid().ToString("n").Substring(0, 30);
                salaryLedger.DateAndTime = DateTime.Now;
                salaryLedger.MidMonth = currentLedger.MidMonth;
                if (salaryLedger.MidMonth == true)
                {
                    salaryLedger.GrossPayPayslip = currentSalaryLedger.GrossPayPayslip;
                }
                else
                {
                    salaryLedger.GrossPayPayslip = 0;
                }
                _context.SalaryLedger.Add(salaryLedger);

                currentLedger.BasicPay = salaryLedger.BasicPay;
                currentLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness;
                currentLedger.DaysOfWorkBP = salaryLedger.DaysOfWorkBP;
                currentLedger.TotalAmountBP = salaryLedger.TotalAmountBP;
                currentLedger.NumberOfMinOT = salaryLedger.NumberOfMinOT;
                currentLedger.AmountOT = salaryLedger.AmountOT;
                currentLedger.NumberOfMinSundays = salaryLedger.NumberOfMinSundays;
                currentLedger.AmountSundays = salaryLedger.AmountSundays;
                currentLedger.GrossPay = salaryLedger.GrossPay;
                currentLedger.GrossPayPayslip = salaryLedger.GrossPayPayslip;
                currentLedger.AmountTardiness = salaryLedger.AmountTardiness;
                currentLedger.TotalDeductions = salaryLedger.TotalDeductions;
                currentLedger.NetAmountPaid = salaryLedger.NetAmountPaid;
                _context.CurrentLedger.Update(currentLedger);
            }
            else
            {
                currentSalaryLedger.NumberOfMinTardiness = currentSalaryLedger.NumberOfMinTardiness + attendance.NumberOfMinTardiness;
                _context.SalaryLedger.Update(currentSalaryLedger);

                //currentLedger.MidMonth = currentSalaryLedger.MidMonth;
                currentLedger.NumberOfMinTardiness = currentSalaryLedger.NumberOfMinTardiness;
                _context.CurrentLedger.Update(currentLedger);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Time in successful!" });
        }

        //POST: api/TimeClock/PostTimeOut
        [HttpPost("PostTimeOut")]
        public async Task<IActionResult> PostTimeOut(string idNumber)
        {
            Employees employees = _context.Employees.Where(x => x.IdNumber == idNumber).FirstOrDefault();
            SalaryLedger currentSalaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == idNumber).OrderByDescending(x => x.DateAndTime).First();
            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == idNumber).FirstOrDefault();
            Attendance attendance = _context.Attendance.Where(x => x.IdNumber == idNumber && x.TimeOut == null).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);
            var totalTimeOut = employees.TotalTimeOut;
            employees.TotalTimeOut = totalTimeOut + 1;

            //var checker = employees.DateTimeChecker.Value.AddHours(1);
            //if (checker > DateTime.Now)
            //{
            //    return Json(new { success = false, message = "Minumum time is 1 hour!" });
            //}
            //else
            //{
            //    employees.DateTimeChecker = DateTime.Now;
            //    _context.Employees.Update(employees);
            //}

            attendance.TimeOut = DateTime.Now;
            attendance.EditorTimeOut = info.FullName;

            TimeSpan diff = attendance.TimeOut.Value - attendance.TimeIn.Value;
            int numberOfMinWorked = (int)diff.TotalMinutes;
            int finalMinWorked = numberOfMinWorked - 60;

            var adjustment = Math.Abs(numberOfMinWorked - 540);

            if (attendance.TimeIn.Value.DayOfWeek != DayOfWeek.Sunday)
            {
                attendance.NumberOfMinWorked = finalMinWorked;
            }

            else if (attendance.TimeIn.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                attendance.NumberOfMinSunday = finalMinWorked;
            }

            attendance.NumberOfMinOT = 0;
            _context.Attendance.Update(attendance);

            currentSalaryLedger.BasicPay = currentSalaryLedger.BasicPay;
            currentSalaryLedger.DaysOfWorkBP = currentSalaryLedger.DaysOfWorkBP + 1;
            currentSalaryLedger.TotalAmountBP = currentSalaryLedger.DaysOfWorkBP * employees.BasicPay.Value;
            currentSalaryLedger.NumberOfMinOT = currentSalaryLedger.NumberOfMinOT + attendance.NumberOfMinOT;
            currentSalaryLedger.AmountOT = employees.BasicPay.Value * 1.25 / 8 / 60 * currentSalaryLedger.NumberOfMinOT;
            currentSalaryLedger.NumberOfMinSundays = currentSalaryLedger.NumberOfMinSundays + attendance.NumberOfMinSunday;
            currentSalaryLedger.AmountSundays = employees.BasicPay.Value * 1.3 / 8 / 60 * currentSalaryLedger.NumberOfMinSundays;
            currentSalaryLedger.GrossPay = currentSalaryLedger.TotalAmountBP + currentSalaryLedger.AmountOT + currentSalaryLedger.AmountSundays + currentSalaryLedger.AmountRH + currentSalaryLedger.AmountSH;
            currentSalaryLedger.GrossPayPayslip = currentSalaryLedger.GrossPayPayslip + currentSalaryLedger.GrossPay;
            currentSalaryLedger.AmountTardiness = currentSalaryLedger.NumberOfMinTardiness * employees.BasicPay.Value / 8 / 60;

            if (currentSalaryLedger.MidMonth == false)
            {
                currentSalaryLedger.PhilHealthEmployee = 0;
                currentSalaryLedger.PhilHealthEmployer = 0;
                currentSalaryLedger.PagibigEmployee = 0;
                currentSalaryLedger.PagibigEmployer = 0;
                currentSalaryLedger.SSSEmployee = 0;
                currentSalaryLedger.SSSEmployer = 0;
                currentSalaryLedger.TotalDeductions = currentSalaryLedger.Charges1 + currentSalaryLedger.AmountTardiness + currentSalaryLedger.CashOut + currentSalaryLedger.PhilHealthEmployee + currentSalaryLedger.PagibigEmployee + currentSalaryLedger.SSSEmployee;
                currentSalaryLedger.NetAmountPaid = currentSalaryLedger.GrossPay - currentSalaryLedger.TotalDeductions;
            }
            else
            {
            if (currentSalaryLedger.GrossPay <= 10000)
            {
                currentSalaryLedger.PhilHealthEmployee = (137.50);
            }
            else if (currentSalaryLedger.GrossPay <= 11000)
            {
                currentSalaryLedger.PhilHealthEmployee = (151.25);
            }
            else if (currentSalaryLedger.GrossPay <= 12000)
            {
                currentSalaryLedger.PhilHealthEmployee = (165);
            }
            else if (currentSalaryLedger.GrossPay <= 13000)
            {
                currentSalaryLedger.PhilHealthEmployee = (178.75);
            }
            else if (currentSalaryLedger.GrossPay <= 14000)
            {
                currentSalaryLedger.PhilHealthEmployee = (192.50);
            }
            else if (currentSalaryLedger.GrossPay <= 15000)
            {
                currentSalaryLedger.PhilHealthEmployee = (206.25);
            }
            else if (currentSalaryLedger.GrossPay <= 16000)
            {
                currentSalaryLedger.PhilHealthEmployee = (220);
            }
            else if (currentSalaryLedger.GrossPay <= 17000)
            {
                currentSalaryLedger.PhilHealthEmployee = (233.75);
            }
            else if (currentSalaryLedger.GrossPay <= 18000)
            {
                currentSalaryLedger.PhilHealthEmployee = (247.50);
            }
            else if (currentSalaryLedger.GrossPay <= 19000)
            {
                currentSalaryLedger.PhilHealthEmployee = (261.25);
            }
            else if (currentSalaryLedger.GrossPay <= 20000)
            {
                currentSalaryLedger.PhilHealthEmployee = (275);
            }
            else if (currentSalaryLedger.GrossPay <= 21000)
            {
                currentSalaryLedger.PhilHealthEmployee = (288.75);
            }
            else if (currentSalaryLedger.GrossPay <= 22000)
            {
                currentSalaryLedger.PhilHealthEmployee = (302.50);
            }
            else if (currentSalaryLedger.GrossPay <= 23000)
            {
                currentSalaryLedger.PhilHealthEmployee = (316.25);
            }
            else if (currentSalaryLedger.GrossPay <= 24000)
            {
                currentSalaryLedger.PhilHealthEmployee = (330);
            }
            else if (currentSalaryLedger.GrossPay <= 25000)
            {
                currentSalaryLedger.PhilHealthEmployee = (343.75);
            }
            else if (currentSalaryLedger.GrossPay <= 26000)
            {
                currentSalaryLedger.PhilHealthEmployee = (357.50);
            }
            else if (currentSalaryLedger.GrossPay <= 27000)
            {
                currentSalaryLedger.PhilHealthEmployee = (371.25);
            }
            else if (currentSalaryLedger.GrossPay <= 28000)
            {
                currentSalaryLedger.PhilHealthEmployee = (385);
            }
            else if (currentSalaryLedger.GrossPay <= 29000)
            {
                currentSalaryLedger.PhilHealthEmployee = (398.75);
            }
            else if (currentSalaryLedger.GrossPay <= 30000)
            {
                currentSalaryLedger.PhilHealthEmployee = (412.50);
            }
            else if (currentSalaryLedger.GrossPay <= 31000)
            {
                currentSalaryLedger.PhilHealthEmployee = (426.25);
            }
            else if (currentSalaryLedger.GrossPay <= 32000)
            {
                currentSalaryLedger.PhilHealthEmployee = (440);
            }
            else if (currentSalaryLedger.GrossPay <= 33000)
            {
                currentSalaryLedger.PhilHealthEmployee = (453.75);
            }
            else if (currentSalaryLedger.GrossPay <= 34000)
            {
                currentSalaryLedger.PhilHealthEmployee = (467.50);
            }
            else if (currentSalaryLedger.GrossPay <= 35000)
            {
                currentSalaryLedger.PhilHealthEmployee = (481.25);
            }
            else if (currentSalaryLedger.GrossPay <= 36000)
            {
                currentSalaryLedger.PhilHealthEmployee = (495);
            }
            else if (currentSalaryLedger.GrossPay <= 37000)
            {
                currentSalaryLedger.PhilHealthEmployee = (508.75);
            }
            else if (currentSalaryLedger.GrossPay <= 38000)
            {
                currentSalaryLedger.PhilHealthEmployee = (522.50);
            }
            else if (currentSalaryLedger.GrossPay <= 39000)
            {
                currentSalaryLedger.PhilHealthEmployee = (536.25);
            }
            else if (currentSalaryLedger.GrossPay <= 39999.99)
            {
                currentSalaryLedger.PhilHealthEmployee = (543.13);
            }
            else
            {
                currentSalaryLedger.PhilHealthEmployee = 550;
            }
            currentSalaryLedger.PhilHealthEmployer = currentSalaryLedger.PhilHealthEmployee;

            if (currentSalaryLedger.GrossPay <= 1500)
            {
                currentSalaryLedger.PagibigEmployer = currentSalaryLedger.GrossPay * .02;
            }
            else
            {
                currentSalaryLedger.PagibigEmployer = currentSalaryLedger.GrossPay * .02;
            }

            if (currentSalaryLedger.GrossPay <= 1500)
            {
                currentSalaryLedger.PagibigEmployee = currentSalaryLedger.GrossPay * .01;
            }
            else
            {
                currentSalaryLedger.PagibigEmployee = currentSalaryLedger.GrossPay * .02;
            }

            if (currentSalaryLedger.GrossPay <= 999)
            {
                currentSalaryLedger.SSSEmployer = 0;
            }
            else if (currentSalaryLedger.GrossPay <= 1249.99)
            {
                currentSalaryLedger.SSSEmployer = (83.70);
            }
            else if (currentSalaryLedger.GrossPay <= 1749.99)
            {
                currentSalaryLedger.SSSEmployer = (120.50);
            }
            else if (currentSalaryLedger.GrossPay <= 2249.99)
            {
                currentSalaryLedger.SSSEmployer = (157.30);
            }
            else if (currentSalaryLedger.GrossPay <= 2749.99)
            {
                currentSalaryLedger.SSSEmployer = (194.20);
            }
            else if (currentSalaryLedger.GrossPay <= 3249.99)
            {
                currentSalaryLedger.SSSEmployer = (231);
            }
            else if (currentSalaryLedger.GrossPay <= 3749.99)
            {
                currentSalaryLedger.SSSEmployer = (267.80);
            }
            else if (currentSalaryLedger.GrossPay <= 4249.99)
            {
                currentSalaryLedger.SSSEmployer = (304.70);
            }
            else if (currentSalaryLedger.GrossPay <= 4749.99)
            {
                currentSalaryLedger.SSSEmployer = (341.50);
            }
            else if (currentSalaryLedger.GrossPay <= 5249.99)
            {
                currentSalaryLedger.SSSEmployer = (378.30);
            }
            else if (currentSalaryLedger.GrossPay <= 5749.99)
            {
                currentSalaryLedger.SSSEmployer = (415.20);
            }
            else if (currentSalaryLedger.GrossPay <= 6249.99)
            {
                currentSalaryLedger.SSSEmployer = (452);
            }
            else if (currentSalaryLedger.GrossPay <= 6749.99)
            {
                currentSalaryLedger.SSSEmployer = (488.80);
            }
            else if (currentSalaryLedger.GrossPay <= 7249.99)
            {
                currentSalaryLedger.SSSEmployer = (525.70);
            }
            else if (currentSalaryLedger.GrossPay <= 7749.99)
            {
                currentSalaryLedger.SSSEmployer = (562.50);
            }
            else if (currentSalaryLedger.GrossPay <= 8249.99)
            {
                currentSalaryLedger.SSSEmployer = (599.30);
            }
            else if (currentSalaryLedger.GrossPay <= 8749.99)
            {
                currentSalaryLedger.SSSEmployer = (636.20);
            }
            else if (currentSalaryLedger.GrossPay <= 9249.99)
            {
                currentSalaryLedger.SSSEmployer = (673);
            }
            else if (currentSalaryLedger.GrossPay <= 9749.99)
            {
                currentSalaryLedger.SSSEmployer = (709.80);
            }
            else if (currentSalaryLedger.GrossPay <= 10249.99)
            {
                currentSalaryLedger.SSSEmployer = (746.70);
            }
            else if (currentSalaryLedger.GrossPay <= 10749.99)
            {
                currentSalaryLedger.SSSEmployer = (783.50);
            }
            else if (currentSalaryLedger.GrossPay <= 11249.99)
            {
                currentSalaryLedger.SSSEmployer = (820.30);
            }
            else if (currentSalaryLedger.GrossPay <= 11749.99)
            {
                currentSalaryLedger.SSSEmployer = (857.20);
            }
            else if (currentSalaryLedger.GrossPay <= 12249.99)
            {
                currentSalaryLedger.SSSEmployer = (894);
            }
            else if (currentSalaryLedger.GrossPay <= 12749.99)
            {
                currentSalaryLedger.SSSEmployer = (930.80);
            }
            else if (currentSalaryLedger.GrossPay <= 13249.99)
            {
                currentSalaryLedger.SSSEmployer = (967.70);
            }
            else if (currentSalaryLedger.GrossPay <= 13749.99)
            {
                currentSalaryLedger.SSSEmployer = (1004.50);
            }
            else if (currentSalaryLedger.GrossPay <= 14249.99)
            {
                currentSalaryLedger.SSSEmployer = (1041.30);
            }
            else if (currentSalaryLedger.GrossPay <= 14749.99)
            {
                currentSalaryLedger.SSSEmployer = (1078.20);
            }
            else if (currentSalaryLedger.GrossPay <= 15249.99)
            {
                currentSalaryLedger.SSSEmployer = (1135);
            }
            else if (currentSalaryLedger.GrossPay <= 15749.99)
            {
                currentSalaryLedger.SSSEmployer = (1171.80);
            }
            else
            {
                currentSalaryLedger.SSSEmployer = (1208.70);
            }

            if (currentSalaryLedger.GrossPay <= 999)
            {
                currentSalaryLedger.SSSEmployee = (0);
            }
            else if (currentSalaryLedger.GrossPay <= 1249.99)
            {
                currentSalaryLedger.SSSEmployee = (36.30);
            }
            else if (currentSalaryLedger.GrossPay <= 1749.99)
            {
                currentSalaryLedger.SSSEmployee = (54.50);
            }
            else if (currentSalaryLedger.GrossPay <= 2249.99)
            {
                currentSalaryLedger.SSSEmployee = (72.70);
            }
            else if (currentSalaryLedger.GrossPay <= 2749.99)
            {
                currentSalaryLedger.SSSEmployee = (90.80);
            }
            else if (currentSalaryLedger.GrossPay <= 3249.99)
            {
                currentSalaryLedger.SSSEmployee = (109);
            }
            else if (currentSalaryLedger.GrossPay <= 3749.99)
            {
                currentSalaryLedger.SSSEmployee = (127.20);
            }
            else if (currentSalaryLedger.GrossPay <= 4249.99)
            {
                currentSalaryLedger.SSSEmployee = (145.30);
            }
            else if (currentSalaryLedger.GrossPay <= 4749.99)
            {
                currentSalaryLedger.SSSEmployee = (163.50);
            }
            else if (currentSalaryLedger.GrossPay <= 5249.99)
            {
                currentSalaryLedger.SSSEmployee = (181.70);
            }
            else if (currentSalaryLedger.GrossPay <= 5749.99)
            {
                currentSalaryLedger.SSSEmployee = (199.80);
            }
            else if (currentSalaryLedger.GrossPay <= 6249.99)
            {
                currentSalaryLedger.SSSEmployee = (218);
            }
            else if (currentSalaryLedger.GrossPay <= 6749.99)
            {
                currentSalaryLedger.SSSEmployee = (236.20);
            }
            else if (currentSalaryLedger.GrossPay <= 7249.99)
            {
                currentSalaryLedger.SSSEmployee = (254.30);
            }
            else if (currentSalaryLedger.GrossPay <= 7749.99)
            {
                currentSalaryLedger.SSSEmployee = (272.50);
            }
            else if (currentSalaryLedger.GrossPay <= 8249.99)
            {
                currentSalaryLedger.SSSEmployee = (290.70);
            }
            else if (currentSalaryLedger.GrossPay <= 8749.99)
            {
                currentSalaryLedger.SSSEmployee = (308.80);
            }
            else if (currentSalaryLedger.GrossPay <= 9249.99)
            {
                currentSalaryLedger.SSSEmployee = (327);
            }
            else if (currentSalaryLedger.GrossPay <= 9749.99)
            {
                currentSalaryLedger.SSSEmployee = (345.20);
            }
            else if (currentSalaryLedger.GrossPay <= 10249.99)
            {
                currentSalaryLedger.SSSEmployee = (363.30);
            }
            else if (currentSalaryLedger.GrossPay <= 10749.99)
            {
                currentSalaryLedger.SSSEmployee = (381.50);
            }
            else if (currentSalaryLedger.GrossPay <= 11249.99)
            {
                currentSalaryLedger.SSSEmployee = (399.70);
            }
            else if (currentSalaryLedger.GrossPay <= 11749.99)
            {
                currentSalaryLedger.SSSEmployee = (417.80);
            }
            else if (currentSalaryLedger.GrossPay <= 12249.99)
            {
                currentSalaryLedger.SSSEmployee = (436);
            }
            else if (currentSalaryLedger.GrossPay <= 12749.99)
            {
                currentSalaryLedger.SSSEmployee = (454.20);
            }
            else if (currentSalaryLedger.GrossPay <= 13249.99)
            {
                currentSalaryLedger.SSSEmployee = (472.30);
            }
            else if (currentSalaryLedger.GrossPay <= 13749.99)
            {
                currentSalaryLedger.SSSEmployee = (490.50);
            }
            else if (currentSalaryLedger.GrossPay <= 14249.99)
            {
                currentSalaryLedger.SSSEmployee = (508.70);
            }
            else if (currentSalaryLedger.GrossPay <= 14749.99)
            {
                currentSalaryLedger.SSSEmployee = (526.80);
            }
            else if (currentSalaryLedger.GrossPay <= 15249.99)
            {
                currentSalaryLedger.SSSEmployee = (545);
            }
            else if (currentSalaryLedger.GrossPay <= 15749.99)
            {
                currentSalaryLedger.SSSEmployee = (563.20);
            }
            else
            {
                currentSalaryLedger.SSSEmployee = (581.30);
            }

                currentSalaryLedger.TotalDeductions = currentSalaryLedger.Charges1 + currentSalaryLedger.AmountTardiness + currentSalaryLedger.CashOut + currentSalaryLedger.PhilHealthEmployee + currentSalaryLedger.PagibigEmployee + currentSalaryLedger.SSSEmployee;
                currentSalaryLedger.NetAmountPaid = currentSalaryLedger.GrossPayPayslip - currentSalaryLedger.TotalDeductions;
            }

            _context.SalaryLedger.Update(currentSalaryLedger);

            currentLedger.NumberOfMinTardiness = currentSalaryLedger.NumberOfMinTardiness;
            currentLedger.DaysOfWorkBP = currentSalaryLedger.DaysOfWorkBP;
            currentLedger.TotalAmountBP = currentSalaryLedger.TotalAmountBP;
            currentLedger.NumberOfMinOT = currentSalaryLedger.NumberOfMinOT;
            currentLedger.AmountOT = currentSalaryLedger.AmountOT;
            currentLedger.NumberOfMinSundays = currentSalaryLedger.NumberOfMinSundays;
            currentLedger.AmountSundays = currentSalaryLedger.AmountSundays;
            currentLedger.GrossPay = currentSalaryLedger.GrossPay;
            currentLedger.GrossPayPayslip = currentLedger.GrossPay;
            currentLedger.AmountTardiness = currentSalaryLedger.AmountTardiness;
            currentLedger.TotalDeductions = currentSalaryLedger.TotalDeductions;
            currentLedger.NetAmountPaid = currentSalaryLedger.NetAmountPaid;

            currentLedger.PhilHealthEmployee = currentSalaryLedger.PhilHealthEmployee;
            currentLedger.PhilHealthEmployer = currentSalaryLedger.PhilHealthEmployer;
            currentLedger.PagibigEmployee = currentSalaryLedger.PagibigEmployee;
            currentLedger.PagibigEmployer = currentSalaryLedger.PagibigEmployer;
            currentLedger.SSSEmployee = currentSalaryLedger.SSSEmployee;
            currentLedger.SSSEmployer = currentSalaryLedger.SSSEmployer;
            _context.CurrentLedger.Update(currentLedger);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Time out successful!" });
        }

        //POST: api/TimeClock/PostOvertime
        [HttpPost("PostOvertime")]
        public async Task<IActionResult> PostOvertime(string idNumber)
        {
            Employees employees = _context.Employees.Where(x => x.IdNumber == idNumber).FirstOrDefault();
            SalaryLedger currentSalaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == idNumber).OrderByDescending(x => x.DateAndTime).First();
            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == idNumber).FirstOrDefault();
            Attendance attendance = _context.Attendance.Where(x => x.IdNumber == idNumber && x.TimeOut == null).FirstOrDefault();
            var info = await _userManager.GetUserAsync(User);
            var totalTimeOut = employees.TotalTimeOut;
            employees.TotalTimeOut = totalTimeOut + 1;

            //var checker = employees.DateTimeChecker.Value.AddHours(1);
            //if (checker > DateTime.Now)
            //{
            //    return Json(new { success = false, message = "Minumum time is 1 hour!" });
            //}
            //else
            //{
            //    employees.DateTimeChecker = DateTime.Now;
            //    _context.Employees.Update(employees);
            //}

            attendance.TimeOut = DateTime.Now;
            attendance.EditorTimeOut = info.FullName;

            TimeSpan diff = attendance.TimeOut.Value - attendance.TimeIn.Value;
            int numberOfMinWorked = (int)diff.TotalMinutes;
            int finalMinWorked = numberOfMinWorked - 60;

            var adjustment = Math.Abs(numberOfMinWorked - 540);

            if (attendance.TimeIn.Value.DayOfWeek != DayOfWeek.Sunday)
            {
                attendance.NumberOfMinWorked = finalMinWorked;
            }

            else if (attendance.TimeIn.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                attendance.NumberOfMinSunday = finalMinWorked;
            }

            var overtime = DateTime.Now;
            overtime = new DateTime(overtime.Year, overtime.Month, overtime.Day, 17, 00, 00);
            TimeSpan solve = attendance.TimeOut.Value - overtime;
            int overtimeMin = (int)solve.TotalMinutes;

            if (attendance.TimeOut.Value > overtime)
            {
                attendance.NumberOfMinOT = overtimeMin;
            }

            _context.Attendance.Update(attendance);

            currentSalaryLedger.BasicPay = currentSalaryLedger.BasicPay;
            currentSalaryLedger.DaysOfWorkBP = currentSalaryLedger.DaysOfWorkBP + 1;
            currentSalaryLedger.TotalAmountBP = currentSalaryLedger.DaysOfWorkBP * employees.BasicPay.Value;
            currentSalaryLedger.NumberOfMinOT = currentSalaryLedger.NumberOfMinOT + attendance.NumberOfMinOT;
            currentSalaryLedger.AmountOT = employees.BasicPay.Value * 1.25 / 8 / 60 * currentSalaryLedger.NumberOfMinOT;
            currentSalaryLedger.NumberOfMinSundays = currentSalaryLedger.NumberOfMinSundays + attendance.NumberOfMinSunday;
            currentSalaryLedger.AmountSundays = employees.BasicPay.Value * 1.3 / 8 / 60 * currentSalaryLedger.NumberOfMinSundays;
            currentSalaryLedger.GrossPay = currentSalaryLedger.TotalAmountBP + currentSalaryLedger.AmountOT + currentSalaryLedger.AmountSundays + currentSalaryLedger.AmountRH + currentSalaryLedger.AmountSH;
            currentSalaryLedger.GrossPayPayslip = currentSalaryLedger.GrossPayPayslip + currentSalaryLedger.GrossPay;
            currentSalaryLedger.AmountTardiness = currentSalaryLedger.NumberOfMinTardiness * employees.BasicPay.Value / 8 / 60;

            if (currentSalaryLedger.MidMonth == false)
            {
                currentSalaryLedger.PhilHealthEmployee = 0;
                currentSalaryLedger.PhilHealthEmployer = 0;
                currentSalaryLedger.PagibigEmployee = 0;
                currentSalaryLedger.PagibigEmployer = 0;
                currentSalaryLedger.SSSEmployee = 0;
                currentSalaryLedger.SSSEmployer = 0;
                currentSalaryLedger.TotalDeductions = currentSalaryLedger.Charges1 + currentSalaryLedger.AmountTardiness + currentSalaryLedger.CashOut + currentSalaryLedger.PhilHealthEmployee + currentSalaryLedger.PagibigEmployee + currentSalaryLedger.SSSEmployee;
                currentSalaryLedger.NetAmountPaid = currentSalaryLedger.GrossPay - currentSalaryLedger.TotalDeductions;
            }
            else
            {
                if (currentSalaryLedger.GrossPay <= 10000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (137.50);
                }
                else if (currentSalaryLedger.GrossPay <= 11000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (151.25);
                }
                else if (currentSalaryLedger.GrossPay <= 12000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (165);
                }
                else if (currentSalaryLedger.GrossPay <= 13000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (178.75);
                }
                else if (currentSalaryLedger.GrossPay <= 14000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (192.50);
                }
                else if (currentSalaryLedger.GrossPay <= 15000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (206.25);
                }
                else if (currentSalaryLedger.GrossPay <= 16000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (220);
                }
                else if (currentSalaryLedger.GrossPay <= 17000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (233.75);
                }
                else if (currentSalaryLedger.GrossPay <= 18000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (247.50);
                }
                else if (currentSalaryLedger.GrossPay <= 19000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (261.25);
                }
                else if (currentSalaryLedger.GrossPay <= 20000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (275);
                }
                else if (currentSalaryLedger.GrossPay <= 21000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (288.75);
                }
                else if (currentSalaryLedger.GrossPay <= 22000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (302.50);
                }
                else if (currentSalaryLedger.GrossPay <= 23000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (316.25);
                }
                else if (currentSalaryLedger.GrossPay <= 24000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (330);
                }
                else if (currentSalaryLedger.GrossPay <= 25000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (343.75);
                }
                else if (currentSalaryLedger.GrossPay <= 26000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (357.50);
                }
                else if (currentSalaryLedger.GrossPay <= 27000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (371.25);
                }
                else if (currentSalaryLedger.GrossPay <= 28000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (385);
                }
                else if (currentSalaryLedger.GrossPay <= 29000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (398.75);
                }
                else if (currentSalaryLedger.GrossPay <= 30000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (412.50);
                }
                else if (currentSalaryLedger.GrossPay <= 31000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (426.25);
                }
                else if (currentSalaryLedger.GrossPay <= 32000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (440);
                }
                else if (currentSalaryLedger.GrossPay <= 33000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (453.75);
                }
                else if (currentSalaryLedger.GrossPay <= 34000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (467.50);
                }
                else if (currentSalaryLedger.GrossPay <= 35000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (481.25);
                }
                else if (currentSalaryLedger.GrossPay <= 36000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (495);
                }
                else if (currentSalaryLedger.GrossPay <= 37000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (508.75);
                }
                else if (currentSalaryLedger.GrossPay <= 38000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (522.50);
                }
                else if (currentSalaryLedger.GrossPay <= 39000)
                {
                    currentSalaryLedger.PhilHealthEmployee = (536.25);
                }
                else if (currentSalaryLedger.GrossPay <= 39999.99)
                {
                    currentSalaryLedger.PhilHealthEmployee = (543.13);
                }
                else
                {
                    currentSalaryLedger.PhilHealthEmployee = 550;
                }
                currentSalaryLedger.PhilHealthEmployer = currentSalaryLedger.PhilHealthEmployee;

                if (currentSalaryLedger.GrossPay <= 1500)
                {
                    currentSalaryLedger.PagibigEmployer = currentSalaryLedger.GrossPay * .02;
                }
                else
                {
                    currentSalaryLedger.PagibigEmployer = currentSalaryLedger.GrossPay * .02;
                }

                if (currentSalaryLedger.GrossPay <= 1500)
                {
                    currentSalaryLedger.PagibigEmployee = currentSalaryLedger.GrossPay * .01;
                }
                else
                {
                    currentSalaryLedger.PagibigEmployee = currentSalaryLedger.GrossPay * .02;
                }

                if (currentSalaryLedger.GrossPay <= 999)
                {
                    currentSalaryLedger.SSSEmployer = 0;
                }
                else if (currentSalaryLedger.GrossPay <= 1249.99)
                {
                    currentSalaryLedger.SSSEmployer = (83.70);
                }
                else if (currentSalaryLedger.GrossPay <= 1749.99)
                {
                    currentSalaryLedger.SSSEmployer = (120.50);
                }
                else if (currentSalaryLedger.GrossPay <= 2249.99)
                {
                    currentSalaryLedger.SSSEmployer = (157.30);
                }
                else if (currentSalaryLedger.GrossPay <= 2749.99)
                {
                    currentSalaryLedger.SSSEmployer = (194.20);
                }
                else if (currentSalaryLedger.GrossPay <= 3249.99)
                {
                    currentSalaryLedger.SSSEmployer = (231);
                }
                else if (currentSalaryLedger.GrossPay <= 3749.99)
                {
                    currentSalaryLedger.SSSEmployer = (267.80);
                }
                else if (currentSalaryLedger.GrossPay <= 4249.99)
                {
                    currentSalaryLedger.SSSEmployer = (304.70);
                }
                else if (currentSalaryLedger.GrossPay <= 4749.99)
                {
                    currentSalaryLedger.SSSEmployer = (341.50);
                }
                else if (currentSalaryLedger.GrossPay <= 5249.99)
                {
                    currentSalaryLedger.SSSEmployer = (378.30);
                }
                else if (currentSalaryLedger.GrossPay <= 5749.99)
                {
                    currentSalaryLedger.SSSEmployer = (415.20);
                }
                else if (currentSalaryLedger.GrossPay <= 6249.99)
                {
                    currentSalaryLedger.SSSEmployer = (452);
                }
                else if (currentSalaryLedger.GrossPay <= 6749.99)
                {
                    currentSalaryLedger.SSSEmployer = (488.80);
                }
                else if (currentSalaryLedger.GrossPay <= 7249.99)
                {
                    currentSalaryLedger.SSSEmployer = (525.70);
                }
                else if (currentSalaryLedger.GrossPay <= 7749.99)
                {
                    currentSalaryLedger.SSSEmployer = (562.50);
                }
                else if (currentSalaryLedger.GrossPay <= 8249.99)
                {
                    currentSalaryLedger.SSSEmployer = (599.30);
                }
                else if (currentSalaryLedger.GrossPay <= 8749.99)
                {
                    currentSalaryLedger.SSSEmployer = (636.20);
                }
                else if (currentSalaryLedger.GrossPay <= 9249.99)
                {
                    currentSalaryLedger.SSSEmployer = (673);
                }
                else if (currentSalaryLedger.GrossPay <= 9749.99)
                {
                    currentSalaryLedger.SSSEmployer = (709.80);
                }
                else if (currentSalaryLedger.GrossPay <= 10249.99)
                {
                    currentSalaryLedger.SSSEmployer = (746.70);
                }
                else if (currentSalaryLedger.GrossPay <= 10749.99)
                {
                    currentSalaryLedger.SSSEmployer = (783.50);
                }
                else if (currentSalaryLedger.GrossPay <= 11249.99)
                {
                    currentSalaryLedger.SSSEmployer = (820.30);
                }
                else if (currentSalaryLedger.GrossPay <= 11749.99)
                {
                    currentSalaryLedger.SSSEmployer = (857.20);
                }
                else if (currentSalaryLedger.GrossPay <= 12249.99)
                {
                    currentSalaryLedger.SSSEmployer = (894);
                }
                else if (currentSalaryLedger.GrossPay <= 12749.99)
                {
                    currentSalaryLedger.SSSEmployer = (930.80);
                }
                else if (currentSalaryLedger.GrossPay <= 13249.99)
                {
                    currentSalaryLedger.SSSEmployer = (967.70);
                }
                else if (currentSalaryLedger.GrossPay <= 13749.99)
                {
                    currentSalaryLedger.SSSEmployer = (1004.50);
                }
                else if (currentSalaryLedger.GrossPay <= 14249.99)
                {
                    currentSalaryLedger.SSSEmployer = (1041.30);
                }
                else if (currentSalaryLedger.GrossPay <= 14749.99)
                {
                    currentSalaryLedger.SSSEmployer = (1078.20);
                }
                else if (currentSalaryLedger.GrossPay <= 15249.99)
                {
                    currentSalaryLedger.SSSEmployer = (1135);
                }
                else if (currentSalaryLedger.GrossPay <= 15749.99)
                {
                    currentSalaryLedger.SSSEmployer = (1171.80);
                }
                else
                {
                    currentSalaryLedger.SSSEmployer = (1208.70);
                }

                if (currentSalaryLedger.GrossPay <= 999)
                {
                    currentSalaryLedger.SSSEmployee = (0);
                }
                else if (currentSalaryLedger.GrossPay <= 1249.99)
                {
                    currentSalaryLedger.SSSEmployee = (36.30);
                }
                else if (currentSalaryLedger.GrossPay <= 1749.99)
                {
                    currentSalaryLedger.SSSEmployee = (54.50);
                }
                else if (currentSalaryLedger.GrossPay <= 2249.99)
                {
                    currentSalaryLedger.SSSEmployee = (72.70);
                }
                else if (currentSalaryLedger.GrossPay <= 2749.99)
                {
                    currentSalaryLedger.SSSEmployee = (90.80);
                }
                else if (currentSalaryLedger.GrossPay <= 3249.99)
                {
                    currentSalaryLedger.SSSEmployee = (109);
                }
                else if (currentSalaryLedger.GrossPay <= 3749.99)
                {
                    currentSalaryLedger.SSSEmployee = (127.20);
                }
                else if (currentSalaryLedger.GrossPay <= 4249.99)
                {
                    currentSalaryLedger.SSSEmployee = (145.30);
                }
                else if (currentSalaryLedger.GrossPay <= 4749.99)
                {
                    currentSalaryLedger.SSSEmployee = (163.50);
                }
                else if (currentSalaryLedger.GrossPay <= 5249.99)
                {
                    currentSalaryLedger.SSSEmployee = (181.70);
                }
                else if (currentSalaryLedger.GrossPay <= 5749.99)
                {
                    currentSalaryLedger.SSSEmployee = (199.80);
                }
                else if (currentSalaryLedger.GrossPay <= 6249.99)
                {
                    currentSalaryLedger.SSSEmployee = (218);
                }
                else if (currentSalaryLedger.GrossPay <= 6749.99)
                {
                    currentSalaryLedger.SSSEmployee = (236.20);
                }
                else if (currentSalaryLedger.GrossPay <= 7249.99)
                {
                    currentSalaryLedger.SSSEmployee = (254.30);
                }
                else if (currentSalaryLedger.GrossPay <= 7749.99)
                {
                    currentSalaryLedger.SSSEmployee = (272.50);
                }
                else if (currentSalaryLedger.GrossPay <= 8249.99)
                {
                    currentSalaryLedger.SSSEmployee = (290.70);
                }
                else if (currentSalaryLedger.GrossPay <= 8749.99)
                {
                    currentSalaryLedger.SSSEmployee = (308.80);
                }
                else if (currentSalaryLedger.GrossPay <= 9249.99)
                {
                    currentSalaryLedger.SSSEmployee = (327);
                }
                else if (currentSalaryLedger.GrossPay <= 9749.99)
                {
                    currentSalaryLedger.SSSEmployee = (345.20);
                }
                else if (currentSalaryLedger.GrossPay <= 10249.99)
                {
                    currentSalaryLedger.SSSEmployee = (363.30);
                }
                else if (currentSalaryLedger.GrossPay <= 10749.99)
                {
                    currentSalaryLedger.SSSEmployee = (381.50);
                }
                else if (currentSalaryLedger.GrossPay <= 11249.99)
                {
                    currentSalaryLedger.SSSEmployee = (399.70);
                }
                else if (currentSalaryLedger.GrossPay <= 11749.99)
                {
                    currentSalaryLedger.SSSEmployee = (417.80);
                }
                else if (currentSalaryLedger.GrossPay <= 12249.99)
                {
                    currentSalaryLedger.SSSEmployee = (436);
                }
                else if (currentSalaryLedger.GrossPay <= 12749.99)
                {
                    currentSalaryLedger.SSSEmployee = (454.20);
                }
                else if (currentSalaryLedger.GrossPay <= 13249.99)
                {
                    currentSalaryLedger.SSSEmployee = (472.30);
                }
                else if (currentSalaryLedger.GrossPay <= 13749.99)
                {
                    currentSalaryLedger.SSSEmployee = (490.50);
                }
                else if (currentSalaryLedger.GrossPay <= 14249.99)
                {
                    currentSalaryLedger.SSSEmployee = (508.70);
                }
                else if (currentSalaryLedger.GrossPay <= 14749.99)
                {
                    currentSalaryLedger.SSSEmployee = (526.80);
                }
                else if (currentSalaryLedger.GrossPay <= 15249.99)
                {
                    currentSalaryLedger.SSSEmployee = (545);
                }
                else if (currentSalaryLedger.GrossPay <= 15749.99)
                {
                    currentSalaryLedger.SSSEmployee = (563.20);
                }
                else
                {
                    currentSalaryLedger.SSSEmployee = (581.30);
                }

                currentSalaryLedger.TotalDeductions = currentSalaryLedger.Charges1 + currentSalaryLedger.AmountTardiness + currentSalaryLedger.CashOut + currentSalaryLedger.PhilHealthEmployee + currentSalaryLedger.PagibigEmployee + currentSalaryLedger.SSSEmployee;
                currentSalaryLedger.NetAmountPaid = currentSalaryLedger.GrossPayPayslip - currentSalaryLedger.TotalDeductions;
            }

            _context.SalaryLedger.Update(currentSalaryLedger);

            currentLedger.NumberOfMinTardiness = currentSalaryLedger.NumberOfMinTardiness;
            currentLedger.DaysOfWorkBP = currentSalaryLedger.DaysOfWorkBP;
            currentLedger.TotalAmountBP = currentSalaryLedger.TotalAmountBP;
            currentLedger.NumberOfMinOT = currentSalaryLedger.NumberOfMinOT;
            currentLedger.AmountOT = currentSalaryLedger.AmountOT;
            currentLedger.NumberOfMinSundays = currentSalaryLedger.NumberOfMinSundays;
            currentLedger.AmountSundays = currentSalaryLedger.AmountSundays;
            currentLedger.GrossPay = currentSalaryLedger.GrossPay;
            currentLedger.GrossPayPayslip = currentLedger.GrossPay;
            currentLedger.AmountTardiness = currentSalaryLedger.AmountTardiness;
            currentLedger.TotalDeductions = currentSalaryLedger.TotalDeductions;
            currentLedger.NetAmountPaid = currentSalaryLedger.NetAmountPaid;

            currentLedger.PhilHealthEmployee = currentSalaryLedger.PhilHealthEmployee;
            currentLedger.PhilHealthEmployer = currentSalaryLedger.PhilHealthEmployer;
            currentLedger.PagibigEmployee = currentSalaryLedger.PagibigEmployee;
            currentLedger.PagibigEmployer = currentSalaryLedger.PagibigEmployer;
            currentLedger.SSSEmployee = currentSalaryLedger.SSSEmployee;
            currentLedger.SSSEmployer = currentSalaryLedger.SSSEmployer;
            _context.CurrentLedger.Update(currentLedger);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Time out successful!" });
        }

    }
}