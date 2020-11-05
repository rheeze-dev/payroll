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
    [Route("api/Attendance")]
    //[Authorize]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public AttendanceController(ApplicationDbContext context,
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

        // GET: api/Attendance/GetAttendance
        [HttpGet("{organizationId}")]
        public async Task<IActionResult> GetAttendanceAsync([FromRoute]Guid organizationId)
        {
            var info = await _userManager.GetUserAsync(User);

            var allAttendance = _context.Attendance.ToList();
            var myAttendance = _context.Attendance.Where(x => x.IdNumber == info.IdNumber);

            if (info.Role == "Employee")
            {
                return Json(new { data = myAttendance });
            }
            else if (info.Role == "Manager")
            {
                return Json(new { data = allAttendance });
            }
            else
            {
                return Json(new { data = allAttendance });
            }
        }

        // GET: api/Deductions/PostDeductions
        [HttpPost]
        public async Task<IActionResult> PostAttendance([FromBody] JObject model)
        {
            Guid objGuid = Guid.Empty;
            objGuid = Guid.Parse(model["Id"].ToString());
            var info = await _userManager.GetUserAsync(User);

            //int id = 0;
            //id = Convert.ToInt32(model["Id"].ToString());
            Attendance attendance = _context.Attendance.Where(x => x.Id == objGuid).FirstOrDefault();
            var originalTardiness = attendance.TotalNumberOfMinTardiness;
            var originalTimeIn = attendance.TimeInAM;
            var threeDaysAgo = DateTime.Now.AddDays(-3);
            if (originalTimeIn < threeDaysAgo)
            {
                return Json(new { success = false, message = "Maximum time to edit is after 3 days!" });
            }
            attendance.TimeInAM = Convert.ToDateTime(model["TimeIn"].ToString());
            attendance.Remarks = model["Remarks"].ToString();
            if (model["Remarks"].ToString() == "")
            {
                return Json(new { success = false, message = "Remarks cannot be empty!" });
            }

            var newTimeIn = Convert.ToDateTime(model["TimeIn"].ToString());
            var newTimeInDate = newTimeIn.Date;
            if (originalTimeIn.Value.Date != newTimeInDate)
            {
                return Json(new { success = false, message = "You can only edit the time!" });
            }
            //attendance.EditorTimeIn = info.FullName;

            var tardiness = DateTime.Now;
            tardiness = new DateTime(tardiness.Year, tardiness.Month, tardiness.Day, 08, 00, 00);
            TimeSpan solve = attendance.TimeInAM.Value - tardiness;
            int tardinessMin = (int)solve.TotalMinutes;

            SalaryLedger salaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == model["IdNumber"].ToString()).FirstOrDefault();
            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == model["IdNumber"].ToString()).FirstOrDefault();
            Employees employees = _context.Employees.Where(x => x.IdNumber == model["IdNumber"].ToString()).FirstOrDefault();

            if (attendance.TimeInAM.Value > tardiness)
            {
                attendance.TotalNumberOfMinTardiness = tardinessMin;

                salaryLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness - originalTardiness;
                salaryLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness + tardinessMin;
                salaryLedger.AmountTardiness = salaryLedger.NumberOfMinTardiness * employees.BasicPay.Value / 8 / 60;
                salaryLedger.TotalDeductions = salaryLedger.Charges1 + salaryLedger.AmountTardiness + salaryLedger.CashOut + salaryLedger.LoanAmount + salaryLedger.PhilHealthEmployee + salaryLedger.PagibigEmployee + salaryLedger.SSSEmployee;
                salaryLedger.NetAmountPaid = salaryLedger.GrossPay - salaryLedger.TotalDeductions;


                //salaryLedger.AmountTardiness = salaryLedger.AmountTardiness - originalTardinessAmount;
                //salaryLedger.AmountTardiness = newTardinessAmount;

                currentLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness;
                currentLedger.AmountTardiness = salaryLedger.AmountTardiness;
                currentLedger.TotalDeductions = salaryLedger.TotalDeductions;
                currentLedger.NetAmountPaid = salaryLedger.NetAmountPaid;

            }
            else if (attendance.TimeInAM.Value < tardiness)
            {
                attendance.TotalNumberOfMinTardiness = attendance.TotalNumberOfMinTardiness - originalTardiness;
                salaryLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness - originalTardiness;
                salaryLedger.AmountTardiness = salaryLedger.NumberOfMinTardiness * employees.BasicPay.Value / 8 / 60;

                currentLedger.NumberOfMinTardiness = salaryLedger.NumberOfMinTardiness;
                currentLedger.AmountTardiness = salaryLedger.AmountTardiness;
            }

            _context.SalaryLedger.Update(salaryLedger);
            _context.Attendance.Update(attendance);
            _context.CurrentLedger.Update(currentLedger);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

    }
}