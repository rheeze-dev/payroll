using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class EmployersDeduction : BaseEntity
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        public double PhilhealthTotal { get; set; }

        public double PagibigTotal { get; set; }

        public double SssTotal { get; set; }

        public DateTime Date { get; set; }
    }
}
