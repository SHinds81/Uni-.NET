using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NWBA.Web.Helper;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace NWBA.Models
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

        public virtual List<TransactionDto> Transactions { get; set; }

        public Account()
        {
            this.Transactions = new List<TransactionDto>();
        }

        //Adds the transaction to the list
        private void AddServiceFee(decimal amount)
        {
            TransactionDto transaction = new TransactionDto()
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
        public async Task AddTransaction(TransactionDto transaction)
        {
            if (transaction.Type == TransactionType.Transfer)
            {
                //if transfering, pay service fee
                if (transaction.AccountNumber == AccountNumber)
                {
                    if (await HasFreeTransfer() == false)
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
                if (await HasFreeTransfer() == false)
                {
                    AddServiceFee((decimal)0.1);
                }
                Balance -= transaction.Amount;
            }

            if (Transactions != null)
            {
                Transactions.Add(transaction);
            }
            

        }

        private async Task<bool> HasFreeTransfer()
        {
            var response = await NwbaApi.InitializeClient().GetAsync("api/transactions/");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            // Storing the response details recieved from web api.
            var result = response.Content.ReadAsStringAsync().Result;

            var transactions = JsonConvert.DeserializeObject<List<TransactionDto>>(result);

            int servicableTransactions = 0;

            foreach(var transaction in transactions)
            {
                if (transaction.AccountNumber == AccountNumber)
                {
                    servicableTransactions++;
                }
            }

            return (servicableTransactions < 4);
        }


    }
}
