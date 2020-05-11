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
    [Route("api/Dashboard")]
    //[Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public DashboardController(ApplicationDbContext context,
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

        // GET: api/Dashboard/GetDashboard
        [HttpGet("{organizationId}")]
        public IActionResult GetDashboard([FromRoute]Guid organizationId)
        {
            var employees = _context.Employees.ToList();
            return Json(new { data = employees });
        }

        //POST: api/Dashboard/PostDashboard
        [HttpPost]
        public async Task<IActionResult> PostDashboard([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());

            Employees employees = _context.Employees.Where(x => x.IdNumber == model["IdNumber"].ToString()).FirstOrDefault();
            employees.Position = model["Position"].ToString();
            employees.BasicPay = Convert.ToInt32(model["BasicPay"].ToString());
            employees.Role = model["Role"].ToString();
            employees.FullName = model["FullName"].ToString();
            employees.Editor = info.FullName;
            employees.DateTimeEdited = DateTime.Now;
            employees.Id = model["Id"].ToString();
            _context.Employees.Update(employees);

            SalaryLedger salaryLedger = _context.SalaryLedger.Where(x => x.IdNumber == employees.IdNumber).FirstOrDefault();
            salaryLedger.BasicPay = employees.BasicPay.Value;
            _context.SalaryLedger.Update(salaryLedger);

            CurrentLedger currentLedger = _context.CurrentLedger.Where(x => x.IdNumber == employees.IdNumber).FirstOrDefault();
            currentLedger.BasicPay = employees.BasicPay.Value;
            _context.CurrentLedger.Update(currentLedger);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

    }
}