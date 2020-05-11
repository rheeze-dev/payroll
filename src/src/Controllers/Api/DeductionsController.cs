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
    [Route("api/Deductions")]
    //[Authorize]
    public class DeductionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public DeductionsController(ApplicationDbContext context,
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

        // GET: api/Deductions/GetSalaryLedger
        [HttpGet("{organizationId}")]
        public IActionResult GetSalaryLedger([FromRoute]Guid organizationId)
        {
            var currentLedger = _context.CurrentLedger.ToList();
            return Json(new { data = currentLedger });
        }

        // GET: api/Deductions/PostDeductions
        [HttpPost]
        public async Task<IActionResult> PostDeductions([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());

            //Employees employees = new Employees
            //{
            //    Position = model["Position"].ToString(),
            //    BasicPay = Convert.ToInt32(model["BasicPay"].ToString()),
            //    Role = model["Role"].ToString(),
            //    IdNumber = model["IdNumber"].ToString(),
            //    FullName = model["FullName"].ToString()
            //};

            ////employees.Editor = info.FullName;
            ////employees.DateTimeEdited = DateTime.Now;
            ////employees.BirthDate = employees.BirthDate;
            ////employees.Email = employees.Email;
            ////employees.TotalTimeOut = employees.TotalTimeOut;
            ////employees.TotalTimeIn = employees.TotalTimeIn;
            ////employees.DateTimeChecker = employees.DateTimeChecker;
            ////employees.PhoneNumber = employees.PhoneNumber;

            //employees.Id = model["Id"].ToString();
            //_context.Employees.Update(employees);

            SalaryLedger salaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == id.ToString()).FirstOrDefault();
            salaryLedger.Charges1 = Convert.ToInt32(model["Charges1"].ToString());
            salaryLedger.CashOut = Convert.ToInt32(model["CashOut"].ToString());
            salaryLedger.SalaryLoan = Convert.ToInt32(model["SalaryLoan"].ToString());
            salaryLedger.PaymentPlan = model["PaymentPlan"].ToString();
            salaryLedger.TotalDeductions = salaryLedger.Charges1 + salaryLedger.AmountTardiness + salaryLedger.CashOut + salaryLedger.SalaryLoan;
            salaryLedger.Editor = info.FullName;

            _context.SalaryLedger.Update(salaryLedger);

            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == id.ToString()).FirstOrDefault();
            currentLedger.Charges1 = salaryLedger.Charges1;
            currentLedger.CashOut = salaryLedger.CashOut;
            currentLedger.SalaryLoan = salaryLedger.SalaryLoan;
            currentLedger.PaymentPlan = salaryLedger.PaymentPlan;
            currentLedger.TotalDeductions = salaryLedger.TotalDeductions;
            _context.CurrentLedger.Update(currentLedger);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

    }
}