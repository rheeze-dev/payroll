using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace src.Models
{
    public class CurrentLedger : BaseEntity
    {
        [Key]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Id number")]
        public string IdNumber { get; set; }

        [StringLength(100)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Basic pay")]
        public int BasicPay { get; set; }

        [Display(Name = "Total basic pay")]
        public int TotalBasicPay { get; set; }

        [Display(Name = "Days of work")]
        public int DaysOfWorkBP { get; set; }

        [Display(Name = "Total amount")]
        public double TotalAmountBP { get; set; }

        [Display(Name = "Number of minutes")]
        public int NumberOfMinOT { get; set; }

        [Display(Name = "Amount")]
        public double AmountOT { get; set; }

        [Display(Name = "Number of minutes")]
        public int NumberOfMinSundays { get; set; }

        [Display(Name = "Amount")]
        public double AmountSundays { get; set; }

        [Display(Name = "Number of hours")]
        public int NumberOfHrsSH { get; set; }

        [Display(Name = "Amount")]
        public double AmountSH { get; set; }

        [Display(Name = "Number of days")]
        public int NumberOfDaysRH { get; set; }

        [Display(Name = "Amount")]
        public double AmountRH { get; set; }

        [Display(Name = "Add adjustment")]
        public int AddAdjustment { get; set; }

        [Display(Name = "Less adjustment")]
        public int LessAdjustment { get; set; }

        [Display(Name = "Number of minutes")]
        public int NumberOfMinTardiness { get; set; }

        [Display(Name = "Amount")]
        public double AmountTardiness { get; set; }

        [Display(Name = "Gross pay")]
        public double GrossPay { get; set; }

        public double GrossPayPayslip { get; set; }

        [Display(Name = "Charges 1")]
        public int Charges1 { get; set; }

        [Display(Name = "Charges 2")]
        public int Charges2 { get; set; }

        [Display(Name = "Cash out")]
        public int CashOut { get; set; }

        [Display(Name = "Salary loan")]
        public int SalaryLoan { get; set; }

        [Display(Name = "Payment plan")]
        public int? PaymentPlan { get; set; }

        [Display(Name = "Loan amount")]
        public double LoanAmount { get; set; }

        [Display(Name = "Loan balance")]
        public double LoanBalance { get; set; }

        [Display(Name = "Total deductions")]
        public double TotalDeductions { get; set; }

        [Display(Name = "Net amount paid")]
        public double NetAmountPaid { get; set; }

        public double PhilHealthEmployee { get; set; }

        public double PhilHealthEmployer { get; set; }

        public double PagibigEmployee { get; set; }

        public double PagibigEmployer { get; set; }

        public double SSSEmployee { get; set; }

        public double SSSEmployer { get; set; }

        //[Display(Name = "Days of work")]
        //public int DaysOfWork { get; set; }

        public DateTime? DateAndTime { get; set; }

        [Display(Name = "Editor")]
        public string Editor { get; set; }

        public bool SalaryLoanChecker { get; set; }

        public bool MidMonth { get; set; }

        public int Cola { get; set; }
    }
}
