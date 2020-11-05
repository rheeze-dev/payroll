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

        public int NumberOfMinTardinessAM { get; set; }

        public int NumberOfMinTardinessPM { get; set; }

        public int TotalNumberOfMinTardiness { get; set; }

        [Display(Name = "Time in AM")]
        public DateTime? TimeInAM { get; set; }

        [Display(Name = "Time out AM")]
        public DateTime? TimeOutAM { get; set; }

        [Display(Name = "Time in PM")]
        public DateTime? TimeInPM { get; set; }

        [Display(Name = "Time out PM")]
        public DateTime? TimeOutPM { get; set; }

        [Display(Name = "Editor")]
        public string Editor { get; set; }

        //[Display(Name = "Editor")]
        //public string EditorTimeOut { get; set; }

        public string Remarks { get; set; }
    }
}
