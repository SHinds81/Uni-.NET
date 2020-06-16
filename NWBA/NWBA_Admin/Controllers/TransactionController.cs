using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NWBA_Admin.Web.Helper;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using NWBA_Admin.Models;
using NWBA_Admin.ViewModels;
using NWBA_Admin.Attributes;

namespace NWBA_Admin.Controllers
{
    [AuthorizeCustomer]
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //displays the transactions depending on the search
        public async Task<IActionResult> ViewTransactions(SearchTransactionsViewModel searchTransactionsViewModel)
        {
            return View(new SearchTransactionsViewModel(await searchTransactionsViewModel.ValidTransactions()));
        }
    }
}