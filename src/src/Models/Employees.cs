using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Employees : BaseEntity
    {
        [Key]
        [Display(Name = "Id Number")]
        public string IdNumber { get; set; }

        [Display(Name = "Id")]
        public string Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        //[Display(Name = "Position")]
        //public string Position { get; set; }

        [Display(Name = "Basic Pay")]
        public int? BasicPay { get; set; }

        [Display(Name = "Birth Date")]
        public string BirthDate { get; set; }

        [Display(Name = "Total basic pay")]
        public int TotalBasicPay { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        [Display(Name = "Date And Time Edited")]
        public DateTime? DateTimeEdited { get; set; }

        [Display(Name = "Editor")]
        public string Editor { get; set; }

        [Display(Name = "Time in")]
        public int TotalTimeIn { get; set; }

        [Display(Name = "Time out")]
        public int TotalTimeOut { get; set; }

        [Display(Name = "Checker")]
        public DateTime? DateTimeChecker { get; set; }

        [Display(Name = "Time in checker")]
        public DateTime? TimeInChecker { get; set; }

        public int Cola { get; set; }
    }
}
