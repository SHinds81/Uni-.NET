using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWBA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NWBA.Models;
using NWBA.Utilities;
using Newtonsoft.Json;
using NWBA.Attributes;
using NWBA.ViewModels;
using X.PagedList;


namespace NWBA.Controllers
{
    [AuthorizeCustomer]
    public class AccountController : Controller
    {
        //Used to get the accountID in the view
        private const string AccountSessionString = "_CustomerSessionKey";

        //DB Context
        private readonly NwbaContext _context;

        //The currently logged in customerID
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        //The db context
        public AccountController(NwbaContext context) => _context = context;

        public IActionResult Edit()
        {
            return View();
        }

        //views transactions of the customer
        public async Task<IActionResult> ViewTransactions()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(new CustomerViewModel(customer));
        }

        //indexes to pages- max 4 on each page
        [HttpPost]
        public async Task<IActionResult> IndexToViewOrders(CustomerViewModel viewModel)
        {
            var account = await _context.Accounts.FindAsync(viewModel.Account.AccountNumber);
            if (account == null)
                return NotFound();

            //sets the account Number to display
            HttpContext.Session.SetInt32(AccountSessionString, account.AccountNumber);

            return RedirectToAction(nameof(ViewOrders));
        }

        //Displays the correct page
        public async Task<IActionResult> ViewOrders(int? page = 1)
        {
            var account = await _context.Accounts.FindAsync(HttpContext.Session.GetInt32(AccountSessionString));
            if (account == null)
                return RedirectToAction(nameof(ViewTransactions));

            const int pageSize = 4;
            var pagedList = await _context.Transactions.Where(x => x.AccountNumber == account.AccountNumber).
                ToPagedListAsync(page, pageSize);

            var customer = await _context.Customers.FindAsync(CustomerID);
            ViewTransactionsViewModel viewModel = new ViewTransactionsViewModel(pagedList, new CustomerViewModel(customer));
            viewModel.Account = account;

            return View(viewModel);
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(ViewTransactions));
        }
    }
}