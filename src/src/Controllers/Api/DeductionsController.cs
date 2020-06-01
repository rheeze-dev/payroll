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

        // GET: api/Deductions/GetEmployeeDeductions
        [HttpGet("GetEmployeeDeductions")]
        public IActionResult GetEmployeeDeductions([FromRoute]Guid organizationId)
        {
            var list = _context.EmployersDeduction.ToList();
            return Json(new { data = list });
        }

        // GET: api/Deductions/PostDeductions
        [HttpPost]
        public async Task<IActionResult> PostDeductions([FromBody] JObject model)
        {
            string id = "";
            var info = await _userManager.GetUserAsync(User);
            id = model["IdNumber"].ToString();

            SalaryLedger salaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == id.ToString()).FirstOrDefault();
            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == id.ToString()).FirstOrDefault();

            salaryLedger.Charges1 = Convert.ToInt32(model["Charges1"].ToString());
            salaryLedger.CashOut = Convert.ToInt32(model["CashOut"].ToString());
            salaryLedger.SalaryLoan = Convert.ToInt32(model["SalaryLoan"].ToString());
            if (model["PaymentPlan"] != null)
            {
                salaryLedger.PaymentPlan = Convert.ToInt32(model["PaymentPlan"].ToString());
            }
            if (Convert.ToInt32(model["SalaryLoan"].ToString()) != 0 && model["PaymentPlan"] == null)
            {
                return Json(new { success = false, message = "Payment plan cannot be empty!" });
            }
            else if (Convert.ToInt32(model["SalaryLoan"].ToString()) == 0 && model["PaymentPlan"] != null)
            {
                return Json(new { success = false, message = "Salary loan cannot be 0!" });
            }

            salaryLedger.TotalDeductions = salaryLedger.Charges1 + salaryLedger.AmountTardiness + salaryLedger.CashOut + salaryLedger.LoanAmount + salaryLedger.PhilHealthEmployee + salaryLedger.PagibigEmployee + salaryLedger.SSSEmployee;
            salaryLedger.NetAmountPaid = salaryLedger.GrossPay - salaryLedger.TotalDeductions;
            salaryLedger.Editor = info.FullName;
            _context.SalaryLedger.Update(salaryLedger);

            currentLedger.Charges1 = salaryLedger.Charges1;
            currentLedger.CashOut = salaryLedger.CashOut;
            currentLedger.SalaryLoan = salaryLedger.SalaryLoan;
            currentLedger.PaymentPlan = salaryLedger.PaymentPlan;
            currentLedger.TotalDeductions = salaryLedger.TotalDeductions;
            currentLedger.NetAmountPaid = salaryLedger.NetAmountPaid;
            currentLedger.Editor = salaryLedger.Editor;
            _context.CurrentLedger.Update(currentLedger);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

    }
}