using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NWBA_API.Models
{
    public enum AccountType
    {
        Checking = 1,
        Saving = 2
    }

    public class Account
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Account Number")]
        public int AccountNumber { get; set; }

        [Required, Display(Name = "Account Type")]
        public AccountType Type { get; set; }

        [Range(-0.01, Int32.MaxValue, ErrorMessage = "Value can not be a negative number")]
        [Column(TypeName = "money")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        [Required, ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Motify Time")]
        public DateTime ModifyTime { get; set; }
        public int FreeTransactions { get; set; }

        public virtual List<Transaction> Transactions { get; set; }

        //Adds the transaction to the list
        private void AddServiceFee(decimal amount)
        {
            Transaction transaction = new Transaction()
            {
                Type = TransactionType.ServiceCharge,
                AccountNumber = AccountNumber,
                Amount = amount,
                Comment = "Service Change",
                TransactionTime = DateTime.UtcNow,
                ModifyTime = DateTime.UtcNow,
            };

            Transactions.Add(transaction);
        }

        //Adds a transaction to the account
        public void AddTransaction(Transaction transaction)
        {
            if (transaction.Type == TransactionType.Transfer)
            {
                //if transfering, pay service fee
                if (transaction.AccountNumber == AccountNumber)
                {
                    if (!HasFreeTransfer)
                    {
                        AddServiceFee((decimal)0.2);
                    }
                    Balance -= transaction.Amount;
                }
                //if receiving. do not pay service fee
                else if (transaction.DestinationAccountNumber == AccountNumber)
                {
                    Balance += transaction.Amount;
                }
            }
            else if (transaction.Type == TransactionType.Deposit)
            {
                Balance += transaction.Amount;
            }
            else if (transaction.Type == TransactionType.Withdraw)
            {
                if (!HasFreeTransfer)
                {
                    AddServiceFee((decimal)0.1);
                }
                Balance -= transaction.Amount;
            }

            //adds transaction to db
            Transactions.Add(transaction);
        }

        private bool HasFreeTransfer => (Transactions.Where(x => x.AccountNumber == AccountNumber).Count() < 4);


    }
}
