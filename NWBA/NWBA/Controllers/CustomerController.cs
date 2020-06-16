using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NWBA.Data;
using NWBA.Models;
using NWBA.Utilities;
using NWBA.Attributes;
using NWBA.ViewModels;
using SimpleHashing;
using System.Collections.Generic;

namespace NwbaExample.Controllers
{
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        private readonly NwbaContext _context;

        // ReSharper disable once PossibleInvalidOperationException
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        public CustomerController(NwbaContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }
        
        //Views the customers details
        public async Task<IActionResult> ViewDetails()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }

        //Views the page to change the password
        public async Task<IActionResult> ChangePassword()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            var login = _context.Logins.Where(x => x.CustomerID == CustomerID).First();
            return View(new ChangePasswordViewModel(customer, login));
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            ModelState.Clear();
            //gets the customer and their login
            var customer = await _context.Customers.FindAsync(CustomerID);
            var login = _context.Logins.Where(x => x.CustomerID == CustomerID).First();

            //Could be moved to a business object but its only two lines
            if (!viewModel.NewHash.Equals(viewModel.RepeatNewHash))
                ModelState.AddModelError("Password", "The passwords must match");

            if (!ModelState.IsValid)
            {
                return await ChangePassword();
            }

            //Hashes the password and changes the modified date
            login.PasswordHash = PBKDF2.Hash(viewModel.NewHash);
            login.ModifyTime = DateTime.UtcNow;
            //saves the changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewDetails));
        }

        //shows the accounts of the customer with balance etc...
        public async Task<IActionResult> ViewAccounts()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }


        public async Task<IActionResult> EditDetails()
        {
            var customer = await _context.Customers.FindAsync(CustomerID);
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditDetails(Customer cust)
        {
            //Validates and returns errors if any
            ModelState.Clear();
            var customer = await _context.Customers.FindAsync(CustomerID);

            Dictionary<string, string> errors = new Dictionary<string, string>();
            Customer.IsValid(cust, out errors);

            //If errors. display them
            if (errors.Count > 0)
            {
                foreach (KeyValuePair<string, string> entry in errors)
                {
                    ModelState.AddModelError(entry.Key, entry.Value);
                }
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(EditDetails));
            }

            //Updates the customer details and saves the changes in the DB
            customer.UpdateDetails(cust);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ViewDetails));
        }
    }
}