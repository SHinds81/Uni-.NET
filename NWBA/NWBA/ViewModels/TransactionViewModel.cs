using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWBA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWBA.ViewModels
{
    public class TransactionViewModel
    {
        public Customer Customer { get; set; }
        public TransactionDto Transaction { get; set; }
        public List<SelectListItem> Accounts
        {
            get
            {
                if (Customer == null)
                {
                    return null;
                }

                List<SelectListItem> accounts = new List<SelectListItem>();

                foreach(Account account in Customer.Accounts)
                {
                    accounts.Add(new SelectListItem($"Account: {account.AccountNumber}, Balance: {account.Balance}", account.AccountNumber.ToString()));
                }

                return accounts;
            }
            set
            {

            }
        }

        public List<SelectListItem> TransactionTypes
        {
            get
            {
                List<SelectListItem> types = new List<SelectListItem>();
                var values = Enum.GetValues(typeof(TransactionType));

                foreach (var value in values)
                {
                    switch (value)
                    {
                        case TransactionType.Transfer:
                            types.Add(new SelectListItem($"Transfer", value.ToString()));
                            break;

                        case TransactionType.Withdraw:
                            types.Add(new SelectListItem($"Withdraw", value.ToString()));
                            break;

                        case TransactionType.Deposit:
                            types.Add(new SelectListItem($"Deposit", value.ToString()));
                            break;
                    }
                }

                return types;
            }
            set
            {

            }
        }

        public TransactionViewModel() { }

        public TransactionViewModel(Customer customer)
        {
            Customer = customer;
        }
    }
}
