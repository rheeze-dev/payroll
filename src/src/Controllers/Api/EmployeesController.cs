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
    [Route("api/Employees")]
    //[Authorize]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDotnetdesk _dotnetdesk;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public EmployeesController(ApplicationDbContext context,
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

        // GET: api/Employees/GetEmployees
        [HttpGet("{organizationId}")]
        public IActionResult GetEmployees([FromRoute]Guid organizationId)
        {
            var employees = _context.Employees.ToList();
            return Json(new { data = employees });
        }

        //POST: api/Employees/PostEmployees
        [HttpPost]
        public async Task<IActionResult> PostEmployees([FromBody] JObject model)
        {
            int id = 0;
            var info = await _userManager.GetUserAsync(User);
            id = Convert.ToInt32(model["Id"].ToString());

            Employees employees = new Employees
            {
                Role = model["Role"].ToString(),
                DateTimeEdited = Convert.ToDateTime(model["DateTimeEdited"].ToString()),
                Editor = model["Editor"].ToString(),
                IdNumber = model["IdNumber"].ToString(),
                FullName = model["FullName"].ToString(),
                Email = model["Email"].ToString()
            };

            employees.Id = model["Id"].ToString();
            _context.Employees.Update(employees);

            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Saved!" });
        }

        //DELETE: api/Employees/DeleteEmployees
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployees([FromRoute] string id)
        {
            Employees employees = _context.Employees.Where(x => x.IdNumber == id).FirstOrDefault();
            _context.Remove(employees);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Delete success." });
        }

    }
}