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
    [Route("api/SalaryLedger")]
    //[Authorize]
    public class SalaryLedgerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public SalaryLedgerController(ApplicationDbContext context,
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

        // GET: api/SalaryLedger/GetSalaryLedger
        [HttpGet("{organizationId}")]
        public async Task<IActionResult> GetSalaryLedger([FromRoute]Guid organizationId)
        {
            var info = await _userManager.GetUserAsync(User);

            var currentLedger = _context.CurrentLedger.ToList();
            var currentUser = _context.CurrentLedger.Where(x => x.IdNumber == info.IdNumber);

            if (info.Role == "Manager")
            {
                return Json(new { data = currentLedger });
            }
            else if (info.Role == "Employee")
            {
                return Json(new { data = currentUser });
            }
            else
            {
                return Json(new { data = currentUser });
            }
        }

        ////POST: api/SalaryLedger/PostSalaryLedger
        //[HttpPost]
        //public async Task<IActionResult> PostSalaryLedger([FromBody] JObject model)
        //{
        //    int id = 0;
        //    var info = await _userManager.GetUserAsync(User);
        //    id = Convert.ToInt32(model["Id"].ToString());

        //    SalaryLedger salaryLedger = new SalaryLedger
        //    {
        //        DateAndTime = DateTime.Now,
        //        Editor = info.FullName,
        //        FullName = model["FullName"].ToString(),
        //        BasicPay = Convert.ToInt32(model["BasicPay"].ToString()),
        //        DaysOfWorkBP = Convert.ToInt32(model["DaysOfWorkBP"].ToString()),
        //        TotalAmountBP = Convert.ToInt32(model["TotalAmountBP"].ToString()),
        //        NumberOfMinOT = Convert.ToInt32(model["NumberOfMinOT"].ToString()),
        //        AmountOT = Convert.ToInt32(model["AmountOT"].ToString()),
        //        NumberOfMinSundays = Convert.ToInt32(model["NumberOfMinSundays"].ToString()),
        //        AmountSundays = Convert.ToInt32(model["AmountSundays"].ToString()),
        //        NumberOfHrsSH = Convert.ToInt32(model["NumberOfHrsSH"].ToString()),
        //        AmountSH = Convert.ToInt32(model["AmountSH"].ToString()),
        //        NumberOfDaysRH = Convert.ToInt32(model["NumberOfDaysRH"].ToString()),
        //        AmountRH = Convert.ToInt32(model["AmountRH"].ToString()),
        //        AddAdjustment = Convert.ToInt32(model["AddAdjustment"].ToString()),
        //        LessAdjustment = Convert.ToInt32(model["LessAdjustment"].ToString()),
        //        NumberOfMinTardiness = Convert.ToInt32(model["NumberOfMinTardiness"].ToString()),
        //        AmountTardiness = Convert.ToInt32(model["AmountTardiness"].ToString()),
        //        GrossPay = Convert.ToInt32(model["GrossPay"].ToString()),
        //        Charges1 = Convert.ToInt32(model["Charges1"].ToString()),
        //        Charges2 = Convert.ToInt32(model["Charges2"].ToString()),
        //        CashOut = Convert.ToInt32(model["CashOut"].ToString()),
        //        SalaryLoan = Convert.ToInt32(model["SalaryLoan"].ToString()),
        //        PaymentPlan = Convert.ToInt32(model["PaymentPlan"].ToString()),
        //        LoanAmount = Convert.ToInt32(model["LoanAmount"].ToString()),
        //        LoanBalance = Convert.ToInt32(model["LoanBalance"].ToString()),
        //        TotalDeductions = Convert.ToInt32(model["TotalDeductions"].ToString()),
        //        NetAmountPaid = Convert.ToInt32(model["NetAmountPaid"].ToString())

        //    };

        //    if (id == 0)
        //    {
        //        _context.SalaryLedger.Add(salaryLedger);
        //    }
        //    else
        //    {
        //        salaryLedger.IdNumber = id.ToString();
        //        _context.SalaryLedger.Update(salaryLedger);
        //    }

        //    await _context.SaveChangesAsync();
        //    return Json(new { success = true, message = "Successfully Saved!" });
        //}

    }
}