using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.Data;
using src.Models;

namespace src.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Config")]
    [Authorize]
    public class ConfigController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfigController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Config/ChangePersonalProfile
        [HttpPost("ChangePersonalProfile")]
        public async Task<IActionResult> ChangePersonalProfile([FromBody] ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ApplicationUser appUser = _context.ApplicationUser.Where(x => x.Id.Equals(applicationUser.Id)).FirstOrDefault();
                Employees employees = _context.Employees.Where(x => x.IdNumber == appUser.IdNumber).FirstOrDefault();
                appUser.FullName = applicationUser.FullName;
                appUser.PhoneNumber = applicationUser.PhoneNumber;
                appUser.Address = applicationUser.Address;
                appUser.BirthDate = applicationUser.BirthDate;

                employees.FullName = appUser.FullName;
                employees.PhoneNumber = applicationUser.PhoneNumber;
                employees.Address = applicationUser.Address;
                employees.BirthDate = applicationUser.BirthDate;

                _context.Employees.Update(employees);
                _context.Update(appUser);

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Edit data success.", appUser });
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message });
            }


        }
    }
}