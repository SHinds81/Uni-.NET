using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NWBA_Admin.Models;
using NWBA_Admin.Web.Helper;

namespace NWBA_Admin.ViewModels
{
    public class SearchTransactionsViewModel
    {
        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }

        [Display(Name = "From")]
        public DateTime FromDate { get; set; }

        [Display(Name = "To")]
        public DateTime ToDate { get; set; }
        public List<TransactionDto> Transactions { get; set; }
        public TransactionDto Transaction;

        public SearchTransactionsViewModel() { }

        public SearchTransactionsViewModel(List<TransactionDto> transactions)
        {
            Transactions = transactions;
        }

        //Gets the valid transactions depending on the date and customerID
        public async Task<List<TransactionDto>> ValidTransactions()
        {
            var response = await NwbaApi.InitializeClient().GetAsync($"api/transactions/");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            var result = response.Content.ReadAsStringAsync().Result;
            var transactions = JsonConvert.DeserializeObject<List<TransactionDto>>(result);

            response = await NwbaApi.InitializeClient().GetAsync("api/accounts/");

            if (!response.IsSuccessStatusCode)
                throw new Exception();

            result = response.Content.ReadAsStringAsync().Result;
            var accounts = JsonConvert.DeserializeObject<List<AccountDto>>(result);

            List<TransactionDto> validTransactions = new List<TransactionDto>();
            foreach (var account in accounts)
            {
                foreach (var transaction in transactions)
                {
                    //If seaching for customer ID
                    if (account.CustomerID == CustomerID && transaction.AccountNumber == account.AccountNumber)
                    {
                        if ((transaction.TransactionTime >= FromDate && transaction.TransactionTime <= ToDate)
                            || (FromDate == DateTime.MinValue && ToDate == DateTime.MinValue))
                        {
                            validTransactions.Add(transaction);
                        }

                    }
                    //if searching for all customers within the time
                    else if (CustomerID == 0 && transaction.TransactionTime >= FromDate
                        && transaction.TransactionTime <= ToDate)
                    {
                        validTransactions.Add(transaction);
                    }
                    //else if searching for all transactions of a certain customer
                    else if (CustomerID == 0 && FromDate == DateTime.MinValue && ToDate == DateTime.MinValue)
                    {
                        validTransactions.Add(transaction);
                    }
                }
            }

            //removes duplicates
            validTransactions = validTransactions.Distinct().ToList();

            return validTransactions;
        }
    }
}
