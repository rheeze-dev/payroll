using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<src.Models.Organization> Organization { get; set; }

        public DbSet<src.Models.Contact> Contact { get; set; }

        public DbSet<src.Models.Attendance> Attendance { get; set; }

        public DbSet<src.Models.Employees> Employees { get; set; }

        public DbSet<src.Models.SalaryLedger> SalaryLedger { get; set; }

        public DbSet<src.Models.CurrentLedger> CurrentLedger { get; set; }

        public DbSet<src.Models.EmployersDeduction> EmployersDeduction { get; set; }

        public DbSet<src.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
