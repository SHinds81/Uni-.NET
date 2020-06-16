using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using NWBA.Data;

namespace NWBA.Models
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

        //Validates a BillPay instance
        public  static void IsValid(NwbaContext _context, BillPay billPay, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();
            Account account = _context.Accounts.Find(billPay.AccountNumber);
            Account destinationAccount = _context.Accounts.Find(billPay.PayeeID);

            if (destinationAccount == null)
            {
                errors.Add(nameof(billPay.PayeeID), "Destination account doesnt exist");
            }
            else
            {
               if (account.AccountNumber == destinationAccount.AccountNumber)
                    errors.Add(nameof(billPay.PayeeID), "Cannot send to the same account");
            }

            if (billPay.Amount <= 0)
                errors.Add(nameof(billPay.Amount), "Amount must be positive.");


            if (billPay.ScheduleDate < DateTime.Now)
                errors.Add(nameof(billPay.ScheduleDate), "Date cant be in the past");
        }
    }

    

}
