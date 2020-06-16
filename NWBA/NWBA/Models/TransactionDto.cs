using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NWBA.Data;

namespace NWBA.Models
{
    public enum TransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3,
        ServiceCharge = 4,
        BillPay = 5,
    }

    public class TransactionDto
    {
        [Key]
        public int TransactionID { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public TransactionType Type { get; set; }

        [Required, ForeignKey("Account")]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }
        
        [Display(Name = "Destination Account Number")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }

        [Range(0.01, Int32.MaxValue, ErrorMessage = "Value can not be a negative number")]
        [Column(TypeName = "money")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Transaction Time")]
        public DateTime TransactionTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Modify Time")]
        public DateTime ModifyTime { get; set; }

        //validates a transaction instance
        public static void IsValid(NwbaContext _context, TransactionDto transaction, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            Account account = _context.Accounts.Find(transaction.AccountNumber);
            Account destinationAccount = _context.Accounts.Find(transaction.DestinationAccountNumber);

            if (destinationAccount == null && transaction.Type == TransactionType.Transfer)
            {
                errors.Add(nameof(transaction.DestinationAccountNumber), "Destination account doesnt exist");
            }
            else if (destinationAccount != null)
            {
                if (account.AccountNumber == destinationAccount.AccountNumber)
                    errors.Add(nameof(transaction.DestinationAccountNumber), "Cannot send to the same account");
            }


            if (transaction.Amount <= 0)
                errors.Add(nameof(transaction.Amount), "Amount must be positive.");

            if (transaction.Amount > account.Balance && transaction.Type != TransactionType.Deposit)
                errors.Add(nameof(transaction.Amount), "Amount cannot be greater than your balance");

        }
    }
}
