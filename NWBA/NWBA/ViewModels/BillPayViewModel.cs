using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWBA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWBA.ViewModels
{
    public class BillPayViewModel
    {
        public Customer Customer { get; set; }
        public BillPay BillPay { get; set; }
        public List<SelectListItem> Accounts
        {
            get
            {
                if (Customer == null)
                {
                    return null;
                }

                List<SelectListItem> accounts = new List<SelectListItem>();

                foreach (Account account in Customer.Accounts)
                {
                    accounts.Add(new SelectListItem($"Account: {account.AccountNumber}, Balance: {account.Balance}", account.AccountNumber.ToString()));
                }

                return accounts;
            }
            set
            {

            }
        }

        public List<SelectListItem> Types
        {
            get
            {
                List<SelectListItem> types = new List<SelectListItem>();
                var values = Enum.GetValues(typeof(Period));

                foreach (var value in values)
                {
                    switch(value)
                    {
                        case Period.Single:
                            types.Add(new SelectListItem($"Single", value.ToString()));
                            break;

                        case Period.Monthly:
                            types.Add(new SelectListItem($"Monthly", value.ToString()));
                            break;

                        case Period.Yearly:
                            types.Add(new SelectListItem($"Yearly", value.ToString()));
                            break;

                        case Period.Quarterly:
                            types.Add(new SelectListItem($"Quarterly", value.ToString()));
                            break;
                    }
                }

                return types;
            }
            set
            {

            }
        }

        public BillPayViewModel() { }

        public BillPayViewModel(Customer customer)
        {
            Customer = customer;
        }

        public BillPayViewModel(Customer customer, BillPay billPay)
        {
            Customer = customer;
            BillPay = billPay;
        }
    }
}
