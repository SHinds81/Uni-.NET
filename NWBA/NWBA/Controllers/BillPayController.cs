using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA.Data;
using NWBA.Models;
using BusinessLayer;
using Microsoft.Extensions.DependencyInjection;
using NWBA.ViewModels;
using NWBA.Attributes;

namespace NWBA.Controllers
{
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly NwbaContext _context;


        //Currenly logged in customer
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public BillPayController(NwbaContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _serviceScopeFactory = serviceScopeFactory;
        }

        //Displays a list view of all the bills
        public async Task<IActionResult> BillPayListView()
        {
            //Gets all the bills relevant to the customer
            List<BillPay> billPays = new List<BillPay>();
            var customer = await _context.Customers.FindAsync(CustomerID);
            foreach (BillPay billPay in _context.BillPay)
            {
                List<Account> accounts = _context.Accounts.Where(x => x.AccountNumber == billPay.AccountNumber).ToList();
                foreach(Account account in accounts)
                {
                    if (account.CustomerID == customer.CustomerID)
                    {
                        billPays.Add(billPay);
                    }
                }
            }

            //Displays them to the customer in a list
            return View(billPays);
        }

        //edits a bill
        public async Task<IActionResult> EditBillPay(int? id)
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            var billPay = await _context.BillPay.FindAsync(id);

            BillPayViewModel model = new BillPayViewModel(customer, billPay);
            return View(model);
        }

        //Post from edit form
        [HttpPost]
        public async Task<IActionResult> EditBillPay(BillPayViewModel _billPay)
        {
            ModelState.Clear();

            //Validates the edited bill pay
            //Shows errors back if invalid
            Dictionary<string, string> errors = new Dictionary<string, string>();
            BillPay.IsValid(_context, _billPay.BillPay, out errors);

            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in errors)
                {
                    ModelState.AddModelError(entry.Key, entry.Value);
                }
            }

            if (!ModelState.IsValid)
            {
                return await EditBillPay(_billPay.BillPay.BillPayID);
            }

            //Updates the chosen bill
            BillPay billPay = await _context.BillPay.FindAsync(_billPay.BillPay.BillPayID);
            if (billPay == null)
            {
                return await EditBillPay(_billPay.BillPay.BillPayID);
            }

            billPay.AccountNumber = _billPay.BillPay.AccountNumber;
            billPay.PayeeID = _billPay.BillPay.PayeeID;
            billPay.Amount = _billPay.BillPay.Amount;
            billPay.ScheduleDate = _billPay.BillPay.ScheduleDate;
            billPay.Period = _billPay.BillPay.Period;
            billPay.ModifyDate = DateTime.UtcNow;

            //saves changes to the DB
            await _context.SaveChangesAsync();
            //goes back to the list view
            return RedirectToAction("BillPayListView", "BillPay");
        }

        public async Task<IActionResult> CreateBillPay()
        {
            //finds the customer that wants to create a new Bill
            var customer = await _context.Customers.FindAsync(CustomerID);
            var billPayModel = new BillPayViewModel(customer);
            return View(billPayModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBillPay(BillPayViewModel billPayModel)
        {
            ModelState.Clear();
            //Validates and shows errors to the user if there are any
            Dictionary<string, string> errors = new Dictionary<string, string>();
            BillPay.IsValid(_context, billPayModel.BillPay, out errors);

            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in errors)
                {
                    ModelState.AddModelError(entry.Key, entry.Value);
                }
            }

            if (!ModelState.IsValid)
            {
                return await CreateBillPay();
            }

            //creates, adds and saves the new bill
            BillPay billPay = new BillPay()
            {
                AccountNumber = billPayModel.BillPay.AccountNumber,
                PayeeID = billPayModel.BillPay.PayeeID,
                Amount = billPayModel.BillPay.Amount,
                ScheduleDate = billPayModel.BillPay.ScheduleDate,
                Period = billPayModel.BillPay.Period,
                ModifyDate = DateTime.UtcNow

            };

            //adds to database and saves
            _context.BillPay.Add(billPay);
            _context.SaveChanges();
            return RedirectToAction("ViewAccounts", "Customer");
        }

        
    }
}

