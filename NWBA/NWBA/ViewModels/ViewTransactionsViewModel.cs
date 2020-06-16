using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NWBA.Models;
using NWBA.Utilities;
using Newtonsoft.Json;
using NWBA.Attributes;
using NWBA.ViewModels;
using X.PagedList;

namespace NWBA.ViewModels
{
    public class ViewTransactionsViewModel
    {
        public IPagedList<TransactionDto> Transactions { get; set; }
        public CustomerViewModel Customer { get; set; }
        public Account Account { get; set; }

        public ViewTransactionsViewModel() { }

        public ViewTransactionsViewModel(IPagedList<TransactionDto> transactions, CustomerViewModel customer)
        {
            Transactions = transactions;
            Customer = customer;
        }
    }
}
