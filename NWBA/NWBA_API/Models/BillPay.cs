using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using NWBA_API.Attributes;

namespace NWBA_API.Models
{
    public enum Period
    {
        Single = 0,
        Monthly = 1,
        Quarterly = 2,
        Yearly = 3,
    }

    public enum Status
    {
        SCHEDULED = 0,
        PAYED = 1,
        FAILED = 2,
        CANCELLED = 3,
        BLOCKED = 4,
    }

    public class BillPay
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BillPayID { get; set; }

        [Required, ForeignKey("Customer")]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }


        [Required]
        [Display(Name = "Payee ID")]
        public int PayeeID { get; set; }

        [Range(0.01, Int32.MaxValue, ErrorMessage = "Value can not be a negative number")]
        public double Amount { get; set; }

        [CurrentDate(ErrorMessage = "Date can't be in the past")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Schedule Date")]
        public DateTime ScheduleDate { get; set; }

        public Status Status { get; set; }

        public Period Period { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Modify Date")]
        public DateTime ModifyDate { get; set; }
    }
}
