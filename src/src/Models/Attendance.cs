using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class Attendance : BaseEntity
    {
        [Key]
        [Display(Name = "Id")]
        public Guid Id { get; set; }

        [Display(Name = "Id Number")]
        public string IdNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public int NumberOfMinWorked { get; set; }

        public int NumberOfMinSunday { get; set; }

        public int NumberOfMinOT { get; set; }

        public int NumberOfMinTardiness { get; set; }

        [Display(Name = "Time in")]
        public DateTime? TimeIn { get; set; }

        [Display(Name = "Time out")]
        public DateTime? TimeOut { get; set; }

        [Display(Name = "Editor")]
        public string EditorTimeIn { get; set; }

        [Display(Name = "Editor")]
        public string EditorTimeOut { get; set; }
    }
}
